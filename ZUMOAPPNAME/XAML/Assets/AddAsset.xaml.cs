using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAsset : ContentPage
    {
        Asset AssetData;
        AssetManager manager;
        UserManager user_manager;
        SubstationManager Subs;
        DateTime? Warranty = new DateTime(1900, 1, 18);
        DateTime? NoInstallasset = new DateTime(1900, 1, 18);
        public string Ids;
        bool update = false;
        //var value;
        public AddAsset(Asset assetdata)
        {
            InitializeComponent();
            manager = AssetManager.DefaultManager;
            Subs = SubstationManager.DefaultManager;
            user_manager = UserManager.DefaultManager;
            if (assetdata != null)
            {
                AddOrUpdateButton.Text = "Update Asset";
                update = true;
                AssetData = assetdata;
                PopulateDetails(AssetData);
                Ids = assetdata.Id;

            }    
          
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Substation_Picker.ItemsSource = await Subs.GetTodoItemsAsync();
        }
        private void PopulateDetails(Asset data)
        {

            Substation_Picker.SelectedItem = data.SubstationCode;
            Plant_Number_Entry.Text = data.PlantNumber;
            Asset_Equipment_Number_Entry.Text = data.AssetEQNO;
            Equipment_Status_Entry.Text = data.EQStatus;
            Serial_Number_Entry.Text = data.SerialNumber;
            Modifier_Code_Entry.Text = data.ModifierCode;
            Location_Equipment_Number_Entry.Text = data.LocationEquipmentNumber;
            Component_Code_Entry.Text = data.ComponentCode;
            WarrantyDate_Picker.Date = (DateTime)data.WarrantyDate; 
            Equipment_Age_Entry.Text = data.YearManufactured;
            Stock_Code_Entry.Text = data.StockCode;
            Purchase_Order_Number_Entry.Text = data.PurchaseOrderNO;
            Rated_Voltage_Entry.Text = data.RatedVoltage;
            Nominal_Voltage_Entry.Text = data.NominalVoltage;
            Manufacturer_Name_Entry.Text = data.ManufacturerName;
            Manufacturer_Type_Entry.Text = data.ManufacturerType;
            Specification_Title_Entry.Text = data.SpecificationTitle;
            Specification_Number_Entry.Text = data.SpecificationNO;
            Specification_Item_Number_Entry.Text = data.SpecificationItemNO;
            LastInstallDate_Picker.Date = (DateTime)data.LastInstallDate;
            Equipment_Class_Entry.Text = data.EquipmentClass;
            Equipment_Class_Description_Entry.Text = data.EquipmentClassDescription;
        }

        async Task AddItem(Asset item)
        {
            await manager.SaveTaskAsync(item);
           
        }
         
        async void Add_Asset_Clicked(object sender, EventArgs e)
        {
            string selected = null;
           
            if (Substation_Picker.SelectedIndex == -1)
            {
                //selected = "null";
                await DisplayAlert("Error", "Substation Code not selected", "Close");
                return;
            }
            if (Substation_Picker.SelectedIndex != -1)
            {
                selected = Substation_Picker.Items[Substation_Picker.SelectedIndex].ToString();
            }
            
            if (Warranty != null)
            {
                Warranty = WarrantyDate_Picker.Date.ToLocalTime();
            }

            if (NoInstallasset != null) 
            {
                NoInstallasset = LastInstallDate_Picker.Date.ToLocalTime();
            }
            if (String.IsNullOrWhiteSpace(Serial_Number_Entry.Text))
            {
                await DisplayAlert("Alert", "Please enter a Serial Number", "OK");
                return;
            }
            if (String.IsNullOrWhiteSpace(Location_Equipment_Number_Entry.Text))
            {
                await DisplayAlert("Alert", "Please enter a Location Equipment number", "OK");
                return;
            }
            if (String.IsNullOrWhiteSpace(Rated_Voltage_Entry.Text))
            {
                await DisplayAlert("Alert", "Please enter a Rated Voltage", "OK");
                return;
            }
            if (String.IsNullOrWhiteSpace(Nominal_Voltage_Entry.Text))
            {
                await DisplayAlert("Alert", "Please enter a Nominal Voltage", "OK");
                return;
            }
            if (String.IsNullOrWhiteSpace(Manufacturer_Name_Entry.Text))
            {
                await DisplayAlert("Alert", "Please enter a Manufacturer Name", "OK");
                return;
            }
            if (String.IsNullOrWhiteSpace(Manufacturer_Type_Entry.Text))
            {
                await DisplayAlert("Alert", "Please enter a Manufacturer Type", "OK");
                return;
            }
            if (String.IsNullOrWhiteSpace(Equipment_Class_Entry.Text))
            {
                await DisplayAlert("Alert", "Please enter a Equipment class", "OK");
                return;
            }

            if (String.IsNullOrWhiteSpace(Equipment_Class_Description_Entry.Text))
            {
                await DisplayAlert("Alert", "Please enter a  Equipment class description", "OK");
                return;
            }
            int i = 0;
            if (int.TryParse(Asset_Equipment_Number_Entry.Text,out i) == false)
            {
                await DisplayAlert("Alert", "Please enter a vaild int for Asset Equipment Number", "OK");
                return;
            }

            if (int.TryParse(Equipment_Age_Entry.Text, out i) == false)
            {
                await DisplayAlert("Alert", "Please enter a vaild int for Equipment Age", "OK");
                return;
            }

            if (int.TryParse(Rated_Voltage_Entry.Text, out i) == false)
            {
                await DisplayAlert("Alert", "Please enter a vaild int for Rated Voltage", "OK");
                return;
            }

            if (int.TryParse(Nominal_Voltage_Entry.Text, out i) == false)
            {
                await DisplayAlert("Alert", "Please enter a vaild int for Nominal Voltage", "OK");
                return;
            }



            Asset todo = new Asset
            {
                Id = Ids,
                SubstationCode = selected,
                PlantNumber = Plant_Number_Entry.Text,
                AssetEQNO = Asset_Equipment_Number_Entry.Text,
                EQStatus = Equipment_Status_Entry.Text,
                SerialNumber = Serial_Number_Entry.Text,
                ModifierCode = Modifier_Code_Entry.Text,
                LocationEquipmentNumber = Location_Equipment_Number_Entry.Text,
                ComponentCode = Component_Code_Entry.Text,
                WarrantyDate = Warranty,
                YearManufactured = Equipment_Age_Entry.Text,
                StockCode = Stock_Code_Entry.Text,
                PurchaseOrderNO = Purchase_Order_Number_Entry.Text,
                RatedVoltage = Rated_Voltage_Entry.Text,
                NominalVoltage = Nominal_Voltage_Entry.Text,
                ManufacturerName = Manufacturer_Name_Entry.Text,
                ManufacturerType = Manufacturer_Type_Entry.Text,
                SpecificationTitle = Specification_Title_Entry.Text,
                SpecificationNO = Specification_Number_Entry.Text,
                SpecificationItemNO = Specification_Item_Number_Entry.Text,
                LastInstallDate = NoInstallasset,
                EquipmentClass = Equipment_Class_Entry.Text,
                EquipmentClassDescription = Equipment_Class_Description_Entry.Text,
            };
            if (update == false) //a brand new asset is being added
            {
                todo.Status = "Added";
                todo.AddedBy = user_manager.ReturnName();
            }
            else
            {
                todo.Status = AssetData.Status; 
                todo.AddedBy = AssetData.AddedBy; 
                todo.ModifiedBy = user_manager.ReturnName();
            }

            await AddItem(todo);
            await Navigation.PushAsync(new AssetList());
        }

        private void NoWarranty_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Warranty = null;
            
        }

        private void NoInstall_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            NoInstallasset = null;
        }
    }
}