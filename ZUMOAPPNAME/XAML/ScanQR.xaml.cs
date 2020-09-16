using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        AssetManager manager;
        public ScanQR()
        {
            manager = AssetManager.DefaultManager;
            InitializeComponent();
        }
        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                
                string search = result.ToString();
                ObservableCollection<Asset> resultscan = await manager.GetScan(search);
                Asset scan = resultscan.FirstOrDefault();
                await Navigation.PushAsync(new Preview_Asset(scan));


    });
        }
    }
}


//<zxing:ZXingDefaultOverlay x:Name="overlay" TopText="Hold your phone up to the barcode" BottomText="Scanning will happen automatically" ShowFlashButton="True" />