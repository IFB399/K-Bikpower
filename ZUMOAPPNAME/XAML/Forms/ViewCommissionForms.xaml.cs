﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCommissionForms : ContentPage
    {
        CommissionManager manager;
        AssetFormLinkManager afl_manager;
        Asset asset;
        public ViewCommissionForms(Asset a = null)
        {
            InitializeComponent();
            manager = CommissionManager.DefaultManager;
            afl_manager = AssetFormLinkManager.DefaultManager;
            asset = a;
            if (a != null)
            {
                assetLabel.Text = "Asset Equipment Number: " + a.AssetEQNO;
                assetLabel.IsVisible = true;
            }
            if (Device.RuntimePlatform == Device.UWP)
            {
                var refreshButton = new Button
                {
                    Text = "Refresh",
                    HeightRequest = 30
                };
                refreshButton.Clicked += OnRefreshItems;
                buttonsPanel.Children.Add(refreshButton);
                if (manager.IsOfflineEnabled)
                {
                    var syncButton = new Button
                    {
                        Text = "Sync items",
                        HeightRequest = 30
                    };
                    syncButton.Clicked += OnSyncItems;
                    buttonsPanel.Children.Add(syncButton);
                }
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            SubmittedBy_Picker.ItemsSource = await manager.GetSubmittedByNames();
            await RefreshItems(true, syncItems: true, a: asset);
        }
        public async void Apply_Filters_Clicked(object sender, EventArgs e)
        {
            string submittedBy = (string)SubmittedBy_Picker.SelectedItem;
            string status = (string)Status_Picker.SelectedItem;

            if (SubmittedBy_Picker.SelectedIndex == -1 && Status_Picker.SelectedIndex == -1)
            {
                FilterLabel.Text = "Filters"; //will improve later
            }
            else
            {
                FilterLabel.Text = "Filters (active)";
            }
            await RefreshItems(true, syncItems: false, submittedBy, status, asset);
            //todoList.ItemsSource = await manager.GetTodoItemsAsync(false, substationCode, equipmentClass, manufacturerName);
        }
        private void Clear_Status_Clicked(object sender, EventArgs e)
        {
            Status_Picker.SelectedIndex = -1;
        }
        private void Clear_SubmittedBy_Clicked(object sender, EventArgs e)
        {
            SubmittedBy_Picker.SelectedIndex = -1;
        }
        private async void Clear_Filters_Clicked(object sender, EventArgs e)
        {
            asset = null;
            assetLabel.IsVisible = false;
            SubmittedBy_Picker.SelectedIndex = -1;
            Status_Picker.SelectedIndex = -1;
            await RefreshItems(true, syncItems: false);
            FilterLabel.Text = "Filters";
        }
        private void Add_Form_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Commission());
        }
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true, true);
        }

        public async void OnRefreshItems(object sender, EventArgs e)
        {
            await RefreshItems(true, false);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems, string submittedBy = null, string status = null, Asset a = null)
        {
            ObservableCollection<string> formIds = null;
            if (a != null)
            {
                formIds = await afl_manager.GetFormIdsByAssetAsync(a.Id, "Commission");
            }
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                formList.ItemsSource = await manager.GetCFormsAsync(syncItems, submittedBy, status, formIds);
            }
        }
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CommissionData c = e.SelectedItem as CommissionData;
            await Navigation.PushAsync(new ApproveCommission(c));
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }

        }
    }
}