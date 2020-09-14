using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Common;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Preview_Asset : ContentPage
    {
        Asset assetdata;
        


        public Preview_Asset(Asset details)
        {
            InitializeComponent();
            if (details != null)
            {
                assetdata = details;
                Console.WriteLine(assetdata);
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
           // WarrantyDate_Picker.Date = details.WarrantyDate;
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
            last_install_date_label.Text = details.LastInstallDate;
            Equipment_class_label.Text = details.EquipmentClass;
            Equipment_class_description_label.Text = details.EquipmentClassDescription;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddAsset(assetdata));
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
            saved.Text = returned;
        }

    }
}
    