using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using ZXing.Net.Mobile.Forms;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormPreviewAsset : ContentPage
    {
        Object savedData;
        ObservableCollection<Asset> assetList = new ObservableCollection<Asset>();
        Asset assetPreviewed;
        //bool prevPage;
        //1 if from scan page
        //2 if from add by table page
        //3 if from manage form assets page
        //4 if from commission approval page
        //5 if from decommission approval page
        int prevPageNumber; 
        public FormPreviewAsset(Asset a, int number=-1, Object o = null, ObservableCollection<Asset> assets = null)
        {
            InitializeComponent();
            savedData = o;
            assetPreviewed = a;
            prevPageNumber = number;
            if (assets != null)
            {
                assetList = assets;
            }
            PopulateDetails(assetPreviewed);
            if (number == 3 || number == 4 || number == 5)
            {
                AddButton.IsVisible = false; //approver is viewing in non edit mode and asset is already added
            }
        }
        private async void Add_Asset_Clicked(object sender, EventArgs e)
        {
            assetList.Add(assetPreviewed); //add asset to list
            await Navigation.PushAsync(new ManageFormAssets(savedData, assetList)); //return to manage form assets page
            //IN CASE I BROKE THIS:
            /*
            if (typeof(DecommissionData).IsInstanceOfType(savedData))
            {
                //DecommissionData d = (DecommissionData)savedData;
                await Navigation.PushAsync(new ManageFormAssets(d, assetList, prevPage)); //return to manage form assets page
            }
            else //return to manage form assets page
            {
                //CommissionData c = (CommissionData)savedData;
                await Navigation.PushAsync(new ManageFormAssets(c, assetList, prevPage));
            }
            */
        }
        private void Go_Back_Clicked(object sender, EventArgs e)
        {
            //return to correct page
            if (prevPageNumber == 1)
            {
                Navigation.PushAsync(new ScanQR(savedData, assetList)); //go to scan page
            }
            else if (prevPageNumber == 2)
            {
                Navigation.PushAsync(new AssetList(savedData, assetList)); //go to table page
            }
            else if (prevPageNumber == 3)
            {
                Navigation.PushAsync(new ManageFormAssets(savedData, assetList)); //go to manage form assets
            }
            else if (prevPageNumber == 4)
            {
                CommissionData c = (CommissionData)savedData;
                Navigation.PushAsync(new ApproveCommission(c, assetList)); //go to approve commission page
            }
            else if (prevPageNumber == 5)
            {
                DecommissionData d = (DecommissionData)savedData;
                Navigation.PushAsync(new ApproveDecommission(d, assetList)); //go to approve decommission page
            }
        }
        private void PopulateDetails(Asset details)
        {
            Status_label.Text = details.Status;
            AddedBy_label.Text = details.AddedBy;
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
    }
}