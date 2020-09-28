using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Preview_Asset : ContentPage
    {
        Asset assetdata;
        AssetManager asset_manager;


        public Preview_Asset(Asset details)
        {
            InitializeComponent();
            asset_manager = AssetManager.DefaultManager;
            if (details != null)
            {
                assetdata = details;
                PopulateDetails(assetdata);
                string QRId = assetdata.Id;
                DependencyService.Get<IQRSave>().Qrcode(QRId);
            }
        }

        private void PopulateDetails(Asset details)
        {
            
            Substation_Code_label.Text = details.SubstationCode;
            Plant_Number_label.Text = details.PlantNumber;
            int AssentEQNO = details.AssetEQNO;
            Asset_EQ_NO_label.Text = AssentEQNO.ToString();
            EQ_Status_label.Text = details.EQStatus;
            Serial_Number_label.Text = details.SerialNumber;
            Modifier_code_label.Text = details.ModifierCode;
            int Loceqnum = details.LocationEquipmentNumber;
            Location_Equipment_Number_label.Text = Loceqnum.ToString();
            Component_Code_label.Text = details.ComponentCode;
            WarrantyDate_Picker.Date = details.Date;
            int EQUage = details.EquipmentAge;
            Equipement_age_label.Text = EQUage.ToString();
            Stock_Code_label.Text = details.StockCode;
            PO_NO_label.Text = details.PurchaseOrderNO;
            int RatedVolts = details.RatedVoltage;
            Rated_Voltage_label.Text = RatedVolts.ToString();
            int NomVolts = details.NominalVoltage;
            Nominal_Voltage_label.Text = NomVolts.ToString();
            Manufacture_Name_label.Text = details.ManufacturerName;
            Specifiaction_title_label.Text = details.SpecificationTitle;
            Specifiaction_NO_label.Text = details.SpecificationNO;
            Specifiaction_item_NO_label.Text = details.SpecificationItemNO;
            LastInstallDate_Picker.Date = details.LastInstallDate;
            Equipment_class_label.Text = details.EquipmentClass;
            Equipment_class_description_label.Text = details.EquipmentClassDescription;
        }
        private void Edit_Asset_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddAsset(assetdata));
        }
        private async void Delete_Asset_Clicked(object sender, EventArgs e)
        {
            //note deleting assets that are included in forms may need special attention
            bool answer = await DisplayAlert("Confirm Asset Deletion", "Delete this asset?", "Yes", "No"); 
            if (answer == true)
            {
                await asset_manager.DeleteAssetAsync(assetdata);
                await Navigation.PushAsync(new AssetList());
            }

        }
        private void DForms_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewDecommissionForms(assetdata));
        }
        private void CForms_Clicked(object sender, EventArgs e)
        {
            //to be completed
        }
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            string QRId = assetdata.Id;
            string QRidcode = QRId.ToString();
            Gen.BarcodeValue = QRidcode;
            Gen.IsVisible = true;
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            //string QRId = assetdata.Id;
            
           string returned = DependencyService.Get<IQRSave>().SaveQrImage().ToString();
           // await DisplayAlert("Alert", "You have been alerted", "OK");
        }

    }
}
    