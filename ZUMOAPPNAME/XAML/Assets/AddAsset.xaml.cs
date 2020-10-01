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
            //WarrantyDate_Picker.Date = data.WarrantyDate; // change dumb dumb. 
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
            //LastInstallDate_Picker.Date = data.LastInstallDate;
            Equipment_Class_Entry.Text = data.EquipmentClass;
            Equipment_Class_Description_Entry.Text = data.EquipmentClassDescription;
        }

        async Task AddItem(Asset item)
        {
            await manager.SaveTaskAsync(item);
            //toDo.ItemsSource = await manager.GetTodoItemsAsync();
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
            //else { value = Substation_Picker.SelectedItem; }
            //DateTime? date = null;
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
                WarrantyDate = WarrantyDate_Picker.Date.ToLocalTime(), // change dumb dumb. 
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
                LastInstallDate = LastInstallDate_Picker.Date.ToLocalTime(),
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
                todo.Status = AssetData.Status; //don't change status
                todo.AddedBy = AssetData.AddedBy; //don't change added by
                todo.ModifiedBy = user_manager.ReturnName();
            }

            await AddItem(todo);
            await Navigation.PushAsync(new AssetList());
        }

    }
}