using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using System.Collections.ObjectModel;
//using Windows.UI.Xaml.Printing;

namespace K_Bikpower
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanQR : ContentPage
    {
        AssetManager manager;
        Object savedData;
        ObservableCollection<Asset> assetList = new ObservableCollection<Asset>();
        public ScanQR(Object o = null, ObservableCollection<Asset> assets = null)
        {
            InitializeComponent();
            manager = AssetManager.DefaultManager;
            savedData = o;
            if (assets != null)
            {
                assetList = assets;
            }
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
                    if (savedData == null) //previous page wasn't a form page
                    {
                        _scanView.IsScanning = false;
                        await Navigation.PushAsync(new Preview_Asset(asset));
                        Navigation.RemovePage(this); //to prevent crashing
                    }
                    else if (typeof(DecommissionData).IsInstanceOfType(savedData))
                    {
                        DecommissionData d = (DecommissionData)savedData;
                        if (assetList.Any((a) => a.Id == asset.Id))
                        {
                            //don't stop scanning
                            await DisplayAlert("Error", "Asset already added", "Close");
                        }
                        else if (asset.Status == "Decommissioned")
                        {
                            //don't stop scanning
                            await DisplayAlert("Asset already decommissioned", "Try commissioning the asset first", "Close");
                        }
                        else
                        {
                            _scanView.IsScanning = false; //stop scanning
                            await Navigation.PushAsync(new FormPreviewAsset(asset, 1,savedData, assetList));
                            Navigation.RemovePage(this); //to prevent crashing
                        }
                    }
                    else //of type commission TO BE COMPLETED
                    {
                        CommissionData c = (CommissionData)savedData;
                        if (assetList.Any((a) => a.Id == asset.Id))
                        {
                            //don't stop scanning
                            await DisplayAlert("Error", "Asset already added", "Close");
                        }
                        else if (asset.Status == "Commissioned")
                        {
                            //don't stop scanning
                            await DisplayAlert("Asset already commissioned", "Try decommissioning the asset first", "Close");
                        }
                        else if (asset.CurrentlyIn == "Scrap")
                        {
                            await DisplayAlert("Asset has been scrapped", "This asset cannot be commissioned", "Close");
                        }
                        else
                        {
                            _scanView.IsScanning = false; //stop scanning
                            //assetList.Add(asset); //also send back asset just scanned
                            //go to preview page actually
                            await Navigation.PushAsync(new FormPreviewAsset(asset, 1,savedData, assetList));
                            Navigation.RemovePage(this); //to prevent crashing
                        }
                    }

                }

            });
            
        }
    }
}