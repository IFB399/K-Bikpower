using System;
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
    public partial class ViewSubstations : ContentPage
    {
        AssetManager asset_manager;
        SubstationManager Subs;
        public ViewSubstations()
        {
            InitializeComponent();
            Subs = SubstationManager.DefaultManager;
            asset_manager = AssetManager.DefaultManager;
            if (Device.RuntimePlatform == Device.UWP)
            {
                var refreshButton = new Button
                {
                    Text = "Refresh",
                    HeightRequest = 30,
                    BackgroundColor = Xamarin.Forms.Color.White,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button))
                };
                refreshButton.Clicked += OnRefreshItems;
                buttonsPanel.Children.Add(refreshButton);
                if (Subs.IsOfflineEnabled)
                {
                    var syncButton = new Button
                    {
                        Text = "Sync items",
                        HeightRequest = 30,
                        BackgroundColor = Xamarin.Forms.Color.White,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button))
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

        private async void ViewAssets(object sender, ItemTappedEventArgs e) //on selected
        {
            
            var detail = e.Item as Substation;
            string details = detail.Substation_Code;
            if (details != null)
            {
                ObservableCollection<string> codes = await asset_manager.GetSubstationCodes();
                if (codes.Contains(detail.Substation_Code))
                {
                    await Navigation.PushAsync(new AssetList(substationCode: detail.Substation_Code));
                }
                else
                {
                    await DisplayAlert("No Assets to View", "There are no assets currently at this substation", "Close");
                }
                //var Assets =  App.Database.GetSubAssetsAsync(details);

 
            }
            /*
            //DELETE CODE (TEMPORARY)
            var detail = e.Item as Substation;
            await Subs.DeleteSubstationAsync(detail);
            */
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
