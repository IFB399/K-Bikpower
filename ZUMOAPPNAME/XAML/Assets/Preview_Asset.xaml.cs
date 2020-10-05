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
            Status_label.Text = details.Status;
            AddedBy_label.Text = details.AddedBy;
            Substation_Code_label.Text = details.SubstationCode;
            Plant_Number_label.Text = details.PlantNumber;
            Asset_EQ_NO_label.Text = details.AssetEQNO;
            EQ_Status_label.Text = details.EQStatus;
            Serial_Number_label.Text = details.SerialNumber;
            Modifier_code_label.Text = details.ModifierCode;
            Location_Equipment_Number_label.Text = details.LocationEquipmentNumber;
            Component_Code_label.Text = details.ComponentCode;
            if (details.WarrantyDate != null)
            {
                WarrantyDate_Picker.Date = (DateTime)details.WarrantyDate;
            }
            else { WarrantyDate_Picker.IsVisible = false; }

            Equipement_age_label.Text = details.YearManufactured;
            Stock_Code_label.Text = details.StockCode;
            PO_NO_label.Text = details.PurchaseOrderNO;
            Rated_Voltage_label.Text = details.RatedVoltage;
            Nominal_Voltage_label.Text = details.NominalVoltage;
            Manufacture_Name_label.Text = details.ManufacturerName;
            Specifiaction_title_label.Text = details.SpecificationTitle;
            Specifiaction_NO_label.Text = details.SpecificationNO;
            Specifiaction_item_NO_label.Text = details.SpecificationItemNO;
            if (details.LastInstallDate != null)
            {
                LastInstallDate_Picker.Date = (DateTime)details.LastInstallDate;
            }
            else { LastInstallDate_Picker.IsVisible = false; }
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
            Navigation.PushAsync(new ViewCommissionForms(assetdata));
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
           string returned = DependencyService.Get<IQRSave>().SaveQrImage().ToString();
           // await DisplayAlert("Alert", "You have been alerted", "OK");
        }

    }
}
    