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
        AssetManager manager;
        public AddAsset()
        {
            InitializeComponent();
            manager = AssetManager.DefaultManager;
        }
        async Task AddItem(Asset item)
        {
            await manager.SaveTaskAsync(item);
            //toDo.ItemsSource = await manager.GetTodoItemsAsync();
        }
        async void Add_Asset_Clicked(object sender, EventArgs e)
        {
            var todo = new Asset {
                SubstationCode = Substation_Picker.Text,
                PlantNumber = Plant_Number_Entry.Text,
                AssetEQNO = Int32.Parse(Asset_Equipment_Number_Entry.Text),
                EQStatus = Equipment_Status_Entry.Text,
                SerialNumber = Serial_Number_Entry.Text,
                ModifierCode = Modifier_Code_Entry.Text,
                LocationEquipmentNumber = Int32.Parse(Location_Equipment_Number_Entry.Text),
                ComponentCode = Component_Code_Entry.Text,
                //WarrantyDate = WarrantyDate_Picker.Date, // change dumb dumb. 
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
                LastInstallDate = Last_Install_Date_Entry.Text,
                EquipmentClass = Equipment_Class_Entry.Text,
                EquipmentClassDescription = Equipment_Class_Description_Entry.Text,
                Status="Added",
                DecommissionFormId=null
                //SubstationCode = "pretty please work", 
                //PlantNumber = "another plant number", 
                //AssetEQNO = 420 
            }; //default asseteqno is 0
            await AddItem(todo);
            await Navigation.PushAsync(new AssetList());
        }

    }
}