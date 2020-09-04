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
            Navigation.PushAsync((new Commission())); 
        }

        private void Decommission_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync((new Decommission())); 
        }
    }
}