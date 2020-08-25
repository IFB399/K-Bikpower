using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Decommission : ContentPage
    {
        ObservableCollection<Asset> assets = new ObservableCollection<Asset>();
        public Decommission()
        {

            InitializeComponent();


            assetList.ItemsSource = assets;
           // assets.Add(new Asset() { id = "329474", Substation_Code = "ALB", Manufacture_Name = "RH" }); //just for display purposes for now
            dateLabel.Text = DateTime.UtcNow.ToString("d");
        }
        private void addAsset_Clicked(object sender, EventArgs e)
        {
            //code breaks when something other than a number is entered
            if (!string.IsNullOrWhiteSpace(AssetEntry.Text))
            {
             //   assets.Add(new Asset() { id = AssetEntry.Text, Substation_Code = "BEL", Manufacture_Name = "ELIN" });
                assetList.HeightRequest += 50; //chose a random number for now, differs between devices
                AssetExpander.ForceUpdateSize();
            }

        }
        private void removeAsset_Clicked(object sender, EventArgs e)
        {
            assets.Remove((Asset)assetList.SelectedItem);
            assetList.HeightRequest -= 50;
            AssetExpander.ForceUpdateSize();
            removeAsset.IsEnabled = false;
        }
        private void selectedAsset(object sender, EventArgs e)
        {
            removeAsset.IsEnabled = true;
        }
    }
}