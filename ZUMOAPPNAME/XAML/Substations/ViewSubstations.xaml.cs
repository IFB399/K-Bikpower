using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSubstations : ContentPage
    {
        SubstationManager Subs;
        public ViewSubstations()
        {
            InitializeComponent();
            Subs = SubstationManager.DefaultManager;
            if (Device.RuntimePlatform == Device.UWP)
            {
                var refreshButton = new Button
                {
                    Text = "Refresh",
                    HeightRequest = 30
                };
                refreshButton.Clicked += OnRefreshItems;
                buttonsPanel.Children.Add(refreshButton);
                if (Subs.IsOfflineEnabled)
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
            await RefreshItems(true, syncItems: true);
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
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Add_Sub(null));
        }

        private void ViewAssets(object sender, ItemTappedEventArgs e)
        {
            var detail = e.Item as Substations;
            //string details = detail.SubstationCode;
           // if (details != null)
            //{

              //  var Assets =  App.Database.GetSubAssetsAsync();
                Navigation.PushAsync(new AssetList());
           // }
        }
        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                AssetsTable.ItemsSource = await Subs.GetTodoItemsAsync(syncItems);
            }
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
