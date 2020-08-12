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
public partial class GeneralDocuments : ContentPage
{
    public GeneralDocuments()
    {
        InitializeComponent();
    }
        private void Commission_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync((new Commission())); //if its not the final page change to navigation page
        }

        private void Decommission_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync((new Decommission())); //if its not the final page change to navigation page
        }
    }
}