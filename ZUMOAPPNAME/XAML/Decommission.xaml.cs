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
        DecommissionManager manager;
        public Decommission()
        {

            InitializeComponent();
            manager = DecommissionManager.DefaultManager;

            assetList.ItemsSource = assets;
           // assets.Add(new Asset() { id = "329474", Substation_Code = "ALB", Manufacture_Name = "RH" }); //just for display purposes for now
            dateLabel.Text = DateTime.UtcNow.ToString("d");
        }
        async Task AddItem(DecommissionData item)
        {
            //await dTable.InsertAsync(item);
            await manager.SaveTaskAsync(item);
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
        private void Scan_Asset_Clicked(object sender, EventArgs e)
        {
            //removeAsset.IsEnabled = true;
        }
        async void Submit_Clicked(object sender, EventArgs e)
        {

            string movedto = "";
            if (Scrap_Button.IsChecked)
            {
                movedto = Scrap_Button.Text;
            }
            if (Project_Button.IsChecked)
            {
                movedto = Project_Button.Text;
            }
            if (Spares_Button.IsChecked)
            {
                movedto = Spares_Button.Text;
            }
            if (Workshop_Button.IsChecked)
            {
                movedto = Workshop_Button.Text;
            }
            var form = new DecommissionData
            {
                Date = Decommissioned_Details_Entry.Text, //will change later
                Details = Decommissioned_Details_Entry.Text,
                RegionName = Region_Picker.SelectedItem.ToString(),
                Location = Location_Entry.Text,
                MovedTo = movedto,
                WorkOrderNumber = Int32.Parse(Work_OrderNo_Entry.Text)


            };
            /*

            var form = new DecommissionData
            {
                Date = "pp", //will change later
                Details = "pp",
                RegionName = "pp",
                Location = "pp",
                MovedTo = "pp",
                WorkOrderNumber = 12
            };
            */
            await AddItem(form);
            //await dTable.InsertAsync(form);
            await Navigation.PushAsync(new MainPage());

        }
    }
}