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
    public partial class Documents : ContentPage
    {
        public Documents()
        {
            InitializeComponent();
        }
        private void General_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync((new GeneralDocuments())); //if its not the final page change to navigation page
        }

    }
}