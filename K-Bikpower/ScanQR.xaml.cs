using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace K_Bikpower
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanQR : ContentPage
    {
        public ScanQR()
        {

            InitializeComponent();
        }
        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                
                string search = result.ToString();
                 var test = App.Database.Scangen(search); //donest work fix later. 

                Assets testing = test[0];
                
                


                await Navigation.PushAsync(new Preview_Asset(testing));
        //await DisplayAlert("Scanned result", search , "OK");


    });
        }
    }
}


//<zxing:ZXingDefaultOverlay x:Name="overlay" TopText="Hold your phone up to the barcode" BottomText="Scanning will happen automatically" ShowFlashButton="True" />