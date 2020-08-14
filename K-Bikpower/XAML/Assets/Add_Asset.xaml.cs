using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K_Bikpower
{
    //variables and event handlers declared in xaml have underscores and data attributes don't
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Add_Asset : ContentPage
    {
        Assets AssetData;
        int Ids;
        public Add_Asset(Assets data)
        {
            InitializeComponent();
            if (data != null)
            {
                AssetData = data;
                PopulateDetails(AssetData);
                Ids = data.Id;
                
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //SubPicker.ItemsSource = await App.Database.GetSubAsync();
            Substation_Picker.ItemsSource = await App.Database.GetSubAsync();
            
        }
        private void PopulateDetails(Assets data)
        {
            
            Substation_Picker.SelectedItem = data;//check 
            Plant_Number_Entry.Text = data.PlantNumber;
            int AssetEQNO = data.AssetEquipmentNumber;
            Asset_Equipment_Number_Entry.Text = AssetEQNO.ToString();
            Equipment_Status_Entry.Text = data.EquipmentStatus;
            Serial_Number_Entry.Text = data.SerialNumber;
            Modifier_Code_Entry.Text = data.ModifierCode;
            int LocationNumber = data.LocationEquipmentNumber;
            Location_Equipment_Number_Entry.Text = LocationNumber.ToString();
            Component_Code_Entry.Text = data.ComponentCode;
            WarrantyDate_Picker.Date = data.WarrantyDate; //not gonna work
            int EquipmentAge = data.EquipmentAge;
            Equipment_Age_Entry.Text = EquipmentAge.ToString();
            Stock_Code_Entry.Text = data.StockCode;
            Purchase_Order_Number_Entry.Text = data.PurchaseOrderNumber;
            int RatedVolts = data.RatedVoltage;
            Rated_Voltage_Entry.Text = RatedVolts.ToString();
            int NomVolts = data.NominalVoltage;
            Nominal_Voltage_Entry.Text = NomVolts.ToString();
            Manufacturer_Name_Entry.Text = data.ManufacturerName;
            Manufacturer_Type_Entry.Text = data.ManufacturerType;
            Specification_Title_Entry.Text = data.SpecificationTitle;
            Specification_Number_Entry.Text = data.SpecificationNumber;
            Specification_Item_Number_Entry.Text = data.SpecificationItemNumber;
            Last_Install_Date_Entry.Text = data.LastInstallDate;
            Equipment_Class_Entry.Text = data.EquipmentClass;
            Equipment_Class_Description_Entry.Text = data.EquipmentClassDescription;
            addassetbutton.Text = "Update";
        }
        async void Add_Asset_Clicked(object sender, EventArgs e)
        {
            //if (!string.IsNullOrWhiteSpace(entry.Text) && !string.IsNullOrWhiteSpace(entry.Text))
            // {
            if (addassetbutton.Text == "Add Asset")
            {
                Boolean x = IsComplete();
                if (x == true)
                {
                    await App.Database.SaveStudentAsync(new Assets
                    {
                        SubstationCode = Substation_Picker.Items[Substation_Picker.SelectedIndex],
                        PlantNumber = Plant_Number_Entry.Text,
                        AssetEquipmentNumber = Int32.Parse(Asset_Equipment_Number_Entry.Text),
                        EquipmentStatus = Equipment_Status_Entry.Text,
                        SerialNumber = Serial_Number_Entry.Text,
                        ModifierCode = Modifier_Code_Entry.Text,
                        LocationEquipmentNumber = Int32.Parse(Location_Equipment_Number_Entry.Text),
                        ComponentCode = Component_Code_Entry.Text,
                        WarrantyDate = WarrantyDate_Picker.Date, // change dumb dumb. 
                        EquipmentAge = Int32.Parse(Equipment_Age_Entry.Text),
                        StockCode = Stock_Code_Entry.Text,
                        PurchaseOrderNumber = Purchase_Order_Number_Entry.Text,
                        RatedVoltage = Int32.Parse(Rated_Voltage_Entry.Text),
                        NominalVoltage = Int32.Parse(Nominal_Voltage_Entry.Text),
                        ManufacturerName = Manufacturer_Name_Entry.Text,
                        ManufacturerType = Manufacturer_Type_Entry.Text,
                        SpecificationTitle = Specification_Title_Entry.Text,
                        SpecificationNumber = Specification_Number_Entry.Text,
                        SpecificationItemNumber = Specification_Item_Number_Entry.Text,
                        LastInstallDate = Last_Install_Date_Entry.Text,
                        EquipmentClass = Equipment_Class_Entry.Text,
                        EquipmentClassDescription = Equipment_Class_Description_Entry.Text,
                    });
                    await Navigation.PushAsync(new View_Assets(null));
                }
                else
                {
                   await DisplayAlert("Alert", "Please ensure all fields are complete", "OK");
                }
                

            }
            else
            {
                await App.Database.UpdateStudentAsync(new Assets //update asset (essentially the same?)
                {

                    Id = Ids,
                    SubstationCode = Substation_Picker.Items[Substation_Picker.SelectedIndex], //is empty when going to update :(
                    PlantNumber = Plant_Number_Entry.Text,
                    AssetEquipmentNumber = Int32.Parse(Asset_Equipment_Number_Entry.Text),
                    EquipmentStatus = Equipment_Status_Entry.Text,
                    SerialNumber = Serial_Number_Entry.Text,
                    ModifierCode = Modifier_Code_Entry.Text,
                    LocationEquipmentNumber = Int32.Parse(Location_Equipment_Number_Entry.Text),
                    ComponentCode = Component_Code_Entry.Text,
                    WarrantyDate = WarrantyDate_Picker.Date, // change dumb dumb. 
                    EquipmentAge = Int32.Parse(Equipment_Age_Entry.Text),
                    StockCode = Stock_Code_Entry.Text,
                    PurchaseOrderNumber = Purchase_Order_Number_Entry.Text,
                    RatedVoltage = Int32.Parse(Rated_Voltage_Entry.Text),
                    NominalVoltage = Int32.Parse(Nominal_Voltage_Entry.Text),
                    ManufacturerName = Manufacturer_Name_Entry.Text,
                    ManufacturerType = Manufacturer_Type_Entry.Text,
                    SpecificationTitle = Specification_Title_Entry.Text,
                    SpecificationNumber = Specification_Number_Entry.Text,
                    SpecificationItemNumber = Specification_Item_Number_Entry.Text,
                    LastInstallDate = Last_Install_Date_Entry.Text,
                    EquipmentClass = Equipment_Class_Entry.Text,
                    EquipmentClassDescription = Equipment_Class_Description_Entry.Text,
                }) ;
                await Navigation.PushAsync(new View_Assets(null));
            }
                // }
                
        }
        //else { await DisplayAlert("Incorrect", "Incorrect username or password", "Close");}

        private Boolean IsComplete()
            //ignoring datepicker check atm. Make sure they don't accidentally always enter the current date?
        {
            if (string.IsNullOrWhiteSpace(Plant_Number_Entry.Text) || string.IsNullOrWhiteSpace(Asset_Equipment_Number_Entry.Text)
                || string.IsNullOrWhiteSpace(Equipment_Status_Entry.Text) || string.IsNullOrWhiteSpace(Serial_Number_Entry.Text)
                || string.IsNullOrWhiteSpace(Modifier_Code_Entry.Text) || string.IsNullOrWhiteSpace(Location_Equipment_Number_Entry.Text)
                || string.IsNullOrWhiteSpace(Component_Code_Entry.Text) || string.IsNullOrWhiteSpace(Equipment_Age_Entry.Text)
                || string.IsNullOrWhiteSpace(Stock_Code_Entry.Text) || string.IsNullOrWhiteSpace(Purchase_Order_Number_Entry.Text)
                || string.IsNullOrWhiteSpace(Rated_Voltage_Entry.Text) || string.IsNullOrWhiteSpace(Nominal_Voltage_Entry.Text)
                || string.IsNullOrWhiteSpace(Manufacturer_Name_Entry.Text) || string.IsNullOrWhiteSpace(Manufacturer_Type_Entry.Text)
                || string.IsNullOrWhiteSpace(Specification_Title_Entry.Text) || string.IsNullOrWhiteSpace(Specification_Number_Entry.Text)
                || string.IsNullOrWhiteSpace(Specification_Item_Number_Entry.Text) || string.IsNullOrWhiteSpace(Last_Install_Date_Entry.Text)
                || string.IsNullOrWhiteSpace(Equipment_Class_Entry.Text) || string.IsNullOrWhiteSpace(Equipment_Class_Description_Entry.Text)
                || Substation_Picker.SelectedIndex == -1)
            {
                return false; //a field has not been filled out
            }
            else
            {
                return true;
            }
        }
    }
}
