using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
//using Windows.Media.Import;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageFormAssets : ContentPage
    {
        ObservableCollection<Asset> globalAssets = new ObservableCollection<Asset>();
 
        Object form; //could be a decommission form or commission form
        
        public ManageFormAssets(Object o, ObservableCollection<Asset> assets)
        {
            InitializeComponent();
            form = o;
            globalAssets = assets;
            assetList.ItemsSource = assets;
        }
        private void removeAsset_Clicked(object sender, EventArgs e)
        {
            globalAssets.Remove((Asset)assetList.SelectedItem);
            removeAsset.IsEnabled = false;
        }
        private async void Asset_Details_Clicked(object sender, EventArgs e)
        {
            AssetDetailsButton.IsEnabled = false;
            //go to preview asset page and indicate that user has come from manage assets page with number 3
            await Navigation.PushAsync(new FormPreviewAsset((Asset)assetList.SelectedItem, 3, form, globalAssets));
        }
        private void selectedAsset(object sender, EventArgs e)
        {
            removeAsset.IsEnabled = true;
            AssetDetailsButton.IsEnabled = true;
        }
        private void Scan_Asset_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScanQR(form, globalAssets)); //go to scan page
        }
        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssetList(form, globalAssets)); //go to table page
        }
        private void Done_Clicked(object sender, EventArgs e)
        {
            if (typeof(DecommissionData).IsInstanceOfType(form))
            {
                DecommissionData d = (DecommissionData)form;
                Navigation.PushAsync(new Decommission(d, globalAssets)); //return to correct page
            }
            else //return to commission page
            {
                CommissionData c = (CommissionData)form;
                Navigation.PushAsync(new Commission(c, globalAssets)); //return to correct page
            }
        }
    }
}