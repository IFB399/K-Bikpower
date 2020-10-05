
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace K_Bikpower
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        UserManager userauth;
        string Authentication;
        public MainPage()
        {
            userauth = UserManager.DefaultManager;
            Authentication = userauth.Authentication();
            InitializeComponent();
            if (Authentication != "Administrator")
            {
                Users.IsVisible = false;
            }
        }

        private void assets_button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssetList()); //if its not the final page change to navigation page
        }

        private void Substation_button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewSubstations()); //if its not the final page change to navigation page
        }

        private void Scan_QRCode_Button(object sender, EventArgs e)
        {
            Navigation.PushAsync((new ScanQR())); //if its not the final page change to navigation page
        }

        private void Documents_Clicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new Documents()); //if its not the final page change to navigation page
        }

        private void Signed_OUT_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new Login()); //if its not the final page change to navigation page
        }

        private void Users_Clicked(object sender, EventArgs e)
        {
            if (Authentication == "Administrator")
            {
                Navigation.PushAsync(new UsersPage());
            }
            else {  DisplayAlert("Denied", "You do not have permssion to access this page", "OK"); }
        }

        private void MyAccount_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PreviewUser(null));
        }
    }
}
