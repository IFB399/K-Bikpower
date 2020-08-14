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
                int qrscan = Int32.Parse(search);
                var test = App.Database.Scangen(qrscan);
                
                if (test.Count == 0)
                {
                    await DisplayAlert("Alert", "Asset could not be found in the database", "OK");
                }
                else
                {
                    Assets testing = test[0];
                    Console.WriteLine(testing);
                    await Navigation.PushAsync(new Preview_Asset(testing));
                }

    });
        }
    }
}


//<zxing:ZXingDefaultOverlay x:Name="overlay" TopText="Hold your phone up to the barcode" BottomText="Scanning will happen automatically" ShowFlashButton="True" />