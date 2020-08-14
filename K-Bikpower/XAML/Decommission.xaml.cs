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
        ObservableCollection<Assets> assets = new ObservableCollection<Assets>();
        public Decommission()
        {

            InitializeComponent();


            assetList.ItemsSource = assets;
            assets.Add(new Assets() { Id = 329474, SubstationCode = "ALB", ManufacturerName = "RH" }); //just for display purposes for now
            dateLabel.Text = DateTime.UtcNow.ToString("d");
        }
        private void Add_Asset_Clicked(object sender, EventArgs e)
        {
            //code breaks when something other than a number is entered
            if (!string.IsNullOrWhiteSpace(AssetEntry.Text))
            {
                assets.Add(new Assets() { Id = Int32.Parse(AssetEntry.Text), SubstationCode = "BEL", ManufacturerName = "ELIN" });
                assetList.HeightRequest += 50; //chose a random number for now, differs between devices
                AssetExpander.ForceUpdateSize();
            }

        }
        private void Remove_Asset_Clicked(object sender, EventArgs e)
        {
            assets.Remove((Assets)assetList.SelectedItem);
            assetList.HeightRequest -= 50;
            AssetExpander.ForceUpdateSize();
            removeAsset.IsEnabled = false;
        }
        private void Selected_Asset(object sender, EventArgs e)
        {
            removeAsset.IsEnabled = true;
        }
    }
}