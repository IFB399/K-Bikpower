using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersPage : ContentPage
    {
        UserManager manager;
        Object savedData;
        public UsersPage()
            {
                InitializeComponent();
             manager = UserManager.DefaultManager;
            
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
            await RefreshItems(true, syncItems: true);
        }

        public async void Add_Asset_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddUser(null));
        }
     
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            User todo = e.SelectedItem as User;
            if (Device.RuntimePlatform != Device.iOS && todo != null)
            {
                if (savedData == null)
                {
                    // Not iOS - the swipe-to-delete is discoverable there
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await Navigation.PushAsync(new PreviewUser(todo));
                    }
                    else
                    {
                        // Windows, not all platforms support the Context Actions yet
                        await Navigation.PushAsync(new PreviewUser(todo));

                    }
                }

            }

            // prevents background getting highlighted
            UserList.SelectedItem = null;
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
                UserList.ItemsSource = await manager.GetTodoItemsAsync();
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