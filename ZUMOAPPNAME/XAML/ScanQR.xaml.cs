using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using System.Collections.ObjectModel;

namespace K_Bikpower
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanQR : ContentPage
    {
        AssetManager manager;
        public ScanQR()
        {
            InitializeComponent();
            manager = AssetManager.DefaultManager;
        }
        public void Handle_OnScanResult(Result result)
        {
            
            Device.BeginInvokeOnMainThread(async () =>
            {
                
                string id = result.ToString();
                ObservableCollection<Asset> a = await manager.GetAsset(id);
                Asset asset = a.FirstOrDefault();
                if (asset == null)
                {
                    await DisplayAlert("Error", "Asset not found", "Close");
                }
                else
                {
                    _scanView.IsScanning = false;
                    await Navigation.PushAsync(new Preview_Asset(asset));
                }
                //int qrscan = Int32.Parse(search);
                // var test = App.Database.Scangen(qrscan);
                //Assets testing = test[0];
                //Console.WriteLine(testing);
            });
            
        }
    }
}


//<zxing:ZXingDefaultOverlay x:Name="overlay" TopText="Hold your phone up to the barcode" BottomText="Scanning will happen automatically" ShowFlashButton="True" />