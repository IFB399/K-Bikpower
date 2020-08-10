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
            var details = App.Database.GetUserAsync();
            string  username = details[0].UserName;
            string password = details[0].Password;
            DateTime lastlogin = details[0].LastLogin;
            if (lastlogin.AddHours(24) > DateTime.UtcNow)
            {
                if (username == "Test" && password == "1234")
                {
                    Navigation.PushAsync(new MainPage());
                }
            }
            else { App.Database.DeleteUser(); };

        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            string username = Username.Text;
            string password = Password.Text;
            if (username == "Test" && password == "1234")
            {
                await App.Database.SaveUserAsync(new User
                {
                    UserName = Username.Text,
                    Password = Password.Text,
                    LastLogin = DateTime.UtcNow,
                }); 
                await Navigation.PushAsync(new MainPage()); 
            }
            
            else { await DisplayAlert("Incorrect", "Incorrect username or password", "Close"); }
        }
    }
}