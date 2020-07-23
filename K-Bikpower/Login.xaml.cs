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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var username = Username.Text;
            var password = Password.Text;
            if (username == "Test" && password == "1234") { Navigation.PushAsync(new MainPage()); }
            else { DisplayAlert("Incorrect", "Incorrect username or password", "Close"); }
        }
    }
}