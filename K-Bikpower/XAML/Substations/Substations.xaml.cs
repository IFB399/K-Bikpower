using K_Bikpower.XAML.Substations;
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
            Navigation.PushAsync(new Add_Sub());
        }
        
        private void ViewAssets(object sender, ItemTappedEventArgs e)
        {
            String details = (string)e.Item;
            if (details != null)
            {

               //var Assets =  App.Database.GetSubAssetsAsync(details);
                Navigation.PushAsync(new View_Assets(details));
            }
        }
    }
}