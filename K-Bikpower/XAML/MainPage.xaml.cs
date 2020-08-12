
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
        public MainPage()
        {
            InitializeComponent();
        }

        private void View_Assets_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new View_Assets(null)); //if its not the final page change to navigation page
        }

        private void View_Substations_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Substations()); //if its not the final page change to navigation page
        }

        private void Scan_QRCode_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync((new ScanQR())); //if its not the final page change to navigation page
        }

        private void View_Documents_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Documents()); //if its not the final page change to navigation page
        }

        private void Manage_Account_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AccountPage()); //if its not the final page change to navigation page
        }

        private void Sign_Out_Clicked(object sender, EventArgs e)
        {
            App.Database.DeleteUser();
            Navigation.PushAsync(new Login()); //if its not the final page change to navigation page
        }
    }
}
