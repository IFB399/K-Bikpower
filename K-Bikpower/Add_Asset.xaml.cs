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
    public partial class Add_Asset : ContentPage
    {
        public Add_Asset(Assets data)
        {
            InitializeComponent();
            if (data != null)
            {

            }
        }
        private void PopulateDetails(Assets data)
        {

        }
    }
}