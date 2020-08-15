using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Preview_Asset : ContentPage
    {
        Assets assetdata;
        


        public Preview_Asset(Assets details)
        {
            InitializeComponent();
            if (details != null)
            {
                assetdata = details;
                Console.WriteLine(assetdata);
                PopulateDetails(assetdata);
            }
        }

        private void PopulateDetails(Assets details)
        {
            
            Substation_Code_label.Text = details.Substation_Code;
            Plant_Number_label.Text = details.Plant_Number;
            int AssentEQNO = details.Asset_EQ_NO;
            Asset_EQ_NO_label.Text = AssentEQNO.ToString();
            EQ_Status_label.Text = details.EQ_Status;
            Serial_Number_label.Text = details.Serial_Number;
            Modifier_code_label.Text = details.Modifier_code;
            int Loceqnum = details.Location_Equipment_Number;
            Location_Equipment_Number_label.Text = Loceqnum.ToString();
            Component_Code_label.Text = details.Component_Code;
           // WarrantyDate_Picker.Date = details.WarrantyDate;
            int EQUage = details.Equipement_age;
            Equipement_age_label.Text = EQUage.ToString();
            Stock_Code_label.Text = details.Stock_Code;
            PO_NO_label.Text = details.PO_NO;
            int RatedVolts = details.Rated_Voltage;
            Rated_Voltage_label.Text = RatedVolts.ToString();
            int NomVolts = details.Nominal_Voltage;
            Nominal_Voltage_label.Text = NomVolts.ToString();
            Manufacture_Name_label.Text = details.Manufacture_Name;
            Specifiaction_title_label.Text = details.Specifiaction_title;
            Specifiaction_NO_label.Text = details.Specifiaction_NO;
            Specifiaction_item_NO_label.Text = details.Specifiaction_item_NO;
            last_install_date_label.Text = details.last_install_date;
            Equipment_class_label.Text = details.Equipment_class;
            Equipment_class_description_label.Text = details.Equipment_class_description;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Add_Asset(assetdata));
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            string QRId = assetdata.Id;
            string QRidcode = QRId.ToString();
            Gen.BarcodeValue = QRidcode;
            Gen.IsVisible = true;
        }
    }
}
    