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
        public View_Assets()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            StudentsTable.ItemsSource = await App.Database.GetPeopleAsync();
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
    }
}