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
    public partial class Preview_Asset : ContentPage
    {
        Assets assetdata;
        public Preview_Asset(Assets details)
        {
            InitializeComponent();
            InitializeComponent();
            if (details != null)
            {
                assetdata = details;
                PopulateDetails(assetdata);
            }
        }
        private void PopulateDetails(Assets details)
        {
            //needs code
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Add_Asset(assetdata));
        }
    }
}