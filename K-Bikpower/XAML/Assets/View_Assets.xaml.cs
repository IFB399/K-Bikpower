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
    public partial class View_Assets : ContentPage
    {
        TableManager Assets;
        public View_Assets(string asset_sub)
        {
            InitializeComponent();
            Assets = TableManager.DefaultManager;
            string asset = asset_sub;
            OnAppearing(asset);
        }
        protected async void OnAppearing(string asset)
        {
            base.OnAppearing();
            if (asset == null)
            { await RefreshItems(true, syncItems: true); }
            else
            {
               // AssetsTable.ItemsSource = await App.Database.GetSubAssetsAsync(asset);
            }

        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Add_Asset(null));
        }
        private void EditAsset(object sender, ItemTappedEventArgs e)
        {
            Assets details = e.Item as Assets;
            if (details != null)
            {
                Navigation.PushAsync(new Preview_Asset(details));
            }
        }
        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                AssetsTable.ItemsSource = await Assets.GetTodoItemsAsync(syncItems);
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