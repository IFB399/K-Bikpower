
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

        private void assets_button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssetList()); //if its not the final page change to navigation page
        }

        private void Substation_button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Substations()); //if its not the final page change to navigation page
        }

        private void Scan_QRCode_Button(object sender, EventArgs e)
        {
            Navigation.PushAsync((new ScanQR())); //if its not the final page change to navigation page
        }

        private void Commision_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync((new Commission())); //if its not the final page change to navigation page
        }

        private void Decommission_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync((new Decommission())); //if its not the final page change to navigation page
        }

        private void View_Clicked(object sender, EventArgs e)
        {
           // Navigation.PushAsync(new TodoList()); //if its not the final page change to navigation page
        }

        private void Signed_OUT_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new Login()); //if its not the final page change to navigation page
        }
    }
}
