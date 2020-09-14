using System;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace K_Bikpower
{
    public partial class AssetList : ContentPage
    {
        AssetManager manager;

        public AssetList()
        {
            InitializeComponent();

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

        // Data methods


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
            //todoList.ItemsSource = await manager.GetTodoItemsAsync(false, substationCode, equipmentClass, manufacturerName);
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

        // Event handlers
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Asset todo = e.SelectedItem as Asset;
            if (Device.RuntimePlatform != Device.iOS && todo != null)
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
            await RefreshItems(true, false);
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

