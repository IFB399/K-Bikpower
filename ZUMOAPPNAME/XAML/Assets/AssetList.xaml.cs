using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;

namespace K_Bikpower
{
    public partial class AssetList : ContentPage
    {
        AssetManager manager;
        Object savedData;
        ObservableCollection<Asset> assetList = new ObservableCollection<Asset>();
        bool prevPage;

        public AssetList(Object o = null, ObservableCollection<Asset> assets = null, bool prevPage2 = false)
        {
            InitializeComponent();
            prevPage = prevPage2;
            manager = AssetManager.DefaultManager;
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
            savedData = o;
            if (assets != null)
            {
                assetList = assets;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ManufacturerPicker.ItemsSource = await manager.GetManufacturerNames();
            SubstationPicker.ItemsSource = await manager.GetSubstationCodes();
            EquipmentClassPicker.ItemsSource = await manager.GetEquipmentClass();
            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            await RefreshItems(true, syncItems: true);
        }

        public async void Add_Asset_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAsset(null));
        }
        public async void Apply_Filters_Clicked(object sender, EventArgs e)
        {
            string substationCode = (string)SubstationPicker.SelectedItem;
            string equipmentClass = (string)EquipmentClassPicker.SelectedItem;
            string manufacturerName = (string)ManufacturerPicker.SelectedItem;
            if (SubstationPicker.SelectedIndex == -1 && EquipmentClassPicker.SelectedIndex == -1 && ManufacturerPicker.SelectedIndex == -1)
            {
                FilterLabel.Text = "Filters"; //will improve later
            }
            else
            {
                FilterLabel.Text = "Filters (active)";
            }
            await RefreshItems(true, syncItems: false, substationCode, equipmentClass, manufacturerName);
        }
        public async void Clear_Filters_Clicked(object sender, EventArgs e)
        {
            ManufacturerPicker.SelectedIndex = -1;
            EquipmentClassPicker.SelectedIndex = -1;
            SubstationPicker.SelectedIndex = -1;
            await RefreshItems(true, syncItems: false);
            FilterLabel.Text = "Filters";
        }
        public void Clear_Substation_Clicked(object sender, EventArgs e)
        {
            SubstationPicker.SelectedIndex = -1;
        }
        public void Clear_EC_Clicked(object sender, EventArgs e)
        {
            EquipmentClassPicker.SelectedIndex = -1;
        }
        public void Clear_Manufacturer_Clicked(object sender, EventArgs e)
        {
            ManufacturerPicker.SelectedIndex = -1;
        }

        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Asset todo = e.SelectedItem as Asset;
            if (Device.RuntimePlatform != Device.iOS && todo != null)
            {
                if (savedData == null)
                {
                    // Not iOS - the swipe-to-delete is discoverable there
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await Navigation.PushAsync(new Preview_Asset(todo));
                    }
                    else
                    {
                        // Windows, not all platforms support the Context Actions yet
                        await Navigation.PushAsync(new Preview_Asset(todo));

                    }
                }
                else if(typeof(DecommissionData).IsInstanceOfType(savedData))
                {
                    DecommissionData d = (DecommissionData)savedData;
                    if (assetList.Any((a) => a.Id == todo.Id))
                    {
                        //_scanView.IsScanning = true;
                        await DisplayAlert("Duplicate Error", "Asset already added", "Close");
                    }
                    else if (todo.Status == "Decommissioned")
                    {
                        await DisplayAlert("Asset already decommissioned", "Try commissioning the asset first", "Close");
                    }
                    else
                    {
                        //assetList.Add(todo);
                        //await Navigation.PushAsync(new ManageFormAssets(d, assetList, prevPage));
                        await Navigation.PushAsync(new FormPreviewAsset(todo, 2,savedData, assetList, prevPage));
                    }

                }
                else if (typeof(CommissionData).IsInstanceOfType(savedData))
                {
                    CommissionData c = (CommissionData)savedData;
                    if (assetList.Any((a) => a.Id == todo.Id))
                    {
                        await DisplayAlert("Duplicate Error", "Asset already added", "Close");
                    }
                    else if (todo.Status == "Commissioned")
                    {
                        await DisplayAlert("Asset already commissioned", "Try decommissioning the asset first", "Close");
                    }
                    else
                    {
                        //assetList.Add(todo);
                        //await Navigation.PushAsync(new ManageFormAssets(c, assetList, prevPage));
                        await Navigation.PushAsync(new FormPreviewAsset(todo, 2,savedData, assetList, prevPage));
                    }

                }

            }

            // prevents background getting highlighted
            todoList.SelectedItem = null;
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
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
            string substationCode = (string)SubstationPicker.SelectedItem;
            string equipmentClass = (string)EquipmentClassPicker.SelectedItem;
            string manufacturerName = (string)ManufacturerPicker.SelectedItem;
            await RefreshItems(true, false, substationCode, equipmentClass, manufacturerName);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems, string substation = null, string equipmentClass = null, string manufacturer = null)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                todoList.ItemsSource = await manager.GetTodoItemsAsync(syncItems, substation, equipmentClass, manufacturer);
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

