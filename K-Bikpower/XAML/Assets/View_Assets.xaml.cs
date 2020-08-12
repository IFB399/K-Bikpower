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
        public View_Assets(string asset_sub)
        {
            InitializeComponent();
            string asset = asset_sub;
            OnAppearing(asset);
        }
        protected async void OnAppearing(string asset)
        {
            base.OnAppearing();
            if(asset == null)
            { AssetsTable.ItemsSource = await App.Database.GetPeopleAsync(); }
            else
            {
                AssetsTable.ItemsSource = await App.Database.GetSubAssetsAsync(asset);
            }
            
        }
        private void Add_Asset_Clicked(object sender, EventArgs e)
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
    }
}