
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
            Navigation.PushAsync(new View_Assets()); //if its not the final page change to navigation page
        }

        private void Substation_button_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new View_Assets()); //if its not the final page change to navigation page
        }

        private void Scan_QRCode_Button(object sender, EventArgs e)
        {
            // Navigation.PushAsync(new View_Assets()); //if its not the final page change to navigation page
        }

        private void Commision_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new View_Assets()); //if its not the final page change to navigation page
        }

        private void Decommission_Clicked(object sender, EventArgs e)
        {
           // Navigation.PushAsync(new View_Assets()); //if its not the final page change to navigation page
        }

        private void View_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new View_Assets()); //if its not the final page change to navigation page
        }

        private void Signed_OUT_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new View_Assets()); //if its not the final page change to navigation page
        }
    }
}
