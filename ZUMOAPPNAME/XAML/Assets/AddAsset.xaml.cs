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
        string value;
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

            Substation_Picker.SelectedItem = data.SubstationCode.ToString();
            Plant_Number_Entry.Text = data.PlantNumber;
            Asset_Equipment_Number_Entry.Text = data.AssetEQNO.ToString();
            Equipment_Status_Entry.Text = data.EQStatus;
            Serial_Number_Entry.Text = data.SerialNumber;
            Modifier_Code_Entry.Text = data.ModifierCode;
            Location_Equipment_Number_Entry.Text = data.LocationEquipmentNumber.ToString();
            Component_Code_Entry.Text = data.ComponentCode;
            WarrantyDate_Picker.Date = data.Date; // change dumb dumb. 
            Equipment_Age_Entry.Text = data.EquipmentAge.ToString();
            Stock_Code_Entry.Text = data.StockCode;
            Purchase_Order_Number_Entry.Text = data.PurchaseOrderNO;
            Rated_Voltage_Entry.Text = data.RatedVoltage.ToString();
            Nominal_Voltage_Entry.Text = data.NominalVoltage.ToString();
            Manufacturer_Name_Entry.Text = data.ManufacturerName;
            Manufacturer_Type_Entry.Text = data.ManufacturerType;
            Specification_Title_Entry.Text = data.SpecificationTitle;
            Specification_Number_Entry.Text = data.SpecificationNO;
            Specification_Item_Number_Entry.Text = data.SpecificationItemNO;
            LastInstallDate_Picker.Date = data.LastInstallDate;
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
            if (Substation_Picker.SelectedItem == null)
            {
                 value = "null";
            }
            else { value = Substation_Picker.SelectedItem.ToString(); }
            Asset todo = new Asset
            {
                Id = Ids,
                SubstationCode = value,
                PlantNumber = Plant_Number_Entry.Text,
                AssetEQNO = Int32.Parse(Asset_Equipment_Number_Entry.Text),
                EQStatus = Equipment_Status_Entry.Text,
                SerialNumber = Serial_Number_Entry.Text,
                ModifierCode = Modifier_Code_Entry.Text,
                LocationEquipmentNumber = Int32.Parse(Location_Equipment_Number_Entry.Text),
                ComponentCode = Component_Code_Entry.Text,
                Date = WarrantyDate_Picker.Date.ToLocalTime(), // change dumb dumb. 
                EquipmentAge = Int32.Parse(Equipment_Age_Entry.Text),
                StockCode = Stock_Code_Entry.Text,
                PurchaseOrderNO = Purchase_Order_Number_Entry.Text,
                RatedVoltage = Int32.Parse(Rated_Voltage_Entry.Text),
                NominalVoltage = Int32.Parse(Nominal_Voltage_Entry.Text),
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