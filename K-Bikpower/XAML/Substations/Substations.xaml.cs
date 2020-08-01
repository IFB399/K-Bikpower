using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//Test
namespace K_Bikpower
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Substations : ContentPage
    {
        public Substations()
        {
            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            AssetsTable.ItemsSource = await App.Database.GetSubAsync();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
          //  Navigation.PushAsync(new Add_Asset(null));
        }
        private void EditAsset(object sender, ItemTappedEventArgs e)
        {
            
        }
    }
}