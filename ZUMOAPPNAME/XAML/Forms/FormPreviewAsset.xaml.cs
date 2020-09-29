﻿using System;
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
        bool prevPage;
        //1 if from scan page
        //2 if from add by table page
        //3 if from manage form assets page
        //4 if from commission approval page
        //5 if from decommission approval page
        int scanPage; //needs a less confusing name
        public FormPreviewAsset(Asset a, int scanPage2=-1, Object o = null, ObservableCollection<Asset> assets = null, bool prevPage2 = false)
        {
            InitializeComponent();
            savedData = o;
            prevPage = prevPage2;
            assetPreviewed = a;
            scanPage = scanPage2;
            if (assets != null)
            {
                assetList = assets;
            }
            PopulateDetails(assetPreviewed);
            if (scanPage == 3 || scanPage == 4)
            {
                AddButton.IsVisible = false;
            }
        }
        private async void Add_Asset_Clicked(object sender, EventArgs e)
        {
            assetList.Add(assetPreviewed); //add asset to list
            if (typeof(DecommissionData).IsInstanceOfType(savedData))
            {
                DecommissionData d = (DecommissionData)savedData;
                await Navigation.PushAsync(new ManageFormAssets(d, assetList, prevPage));
            }
            else //return to commission page
            {
                CommissionData c = (CommissionData)savedData;
                await Navigation.PushAsync(new ManageFormAssets(c, assetList, prevPage));
            }
                
        }
        private void Go_Back_Clicked(object sender, EventArgs e)
        {
            //return to scan page or add by table page
            if (scanPage == 1)
            {
                Navigation.PushAsync(new ScanQR(savedData, assetList, prevPage)); //go to scan page
            }
            else if (scanPage == 2)
            {
                Navigation.PushAsync(new AssetList(savedData, assetList, prevPage)); //go to table page
            }
            else if (scanPage == 3)
            {
                Navigation.PushAsync(new ManageFormAssets(savedData, assetList, prevPage)); //go to manage form assets
            }
            else if (scanPage == 4)
            {
                CommissionData c = (CommissionData)savedData;
                Navigation.PushAsync(new ApproveCommission(c, assetList)); //go to table page
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
    }
}