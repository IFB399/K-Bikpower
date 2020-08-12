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
            SubPicker.ItemsSource = await App.Database.GetSubAsync();
            
        }
        private void PopulateDetails(Assets data)
        {
            
            SubPicker.SelectedItem = data;//check 
            Plant_Number_Entry.Text = data.Plant_Number;
            int AssentEQNO = data.Asset_EQ_NO;
            Asset_EQ_NO_Entry.Text = AssentEQNO.ToString();
            EQ_Status_Entry.Text = data.EQ_Status;
            Serial_Number_Entry.Text = data.Serial_Number;
            Modifier_code_Entry.Text = data.Modifier_code;
            int Loceqnum = data.Location_Equipment_Number;
            Location_Equipment_Number_Entry.Text = Loceqnum.ToString();
            Component_Code_Entry.Text = data.Component_Code;
            WarrantyDate_Picker.Date = data.WarrantyDate; //not gonna work
            int EQUage = data.Equipement_age;
            Equipement_age_Entry.Text = EQUage.ToString();
            Stock_Code_Entry.Text = data.Stock_Code;
            PO_NO_Entry.Text = data.PO_NO;
            int RatedVolts = data.Rated_Voltage;
            Rated_Voltage_Entry.Text = RatedVolts.ToString();
            int NomVolts = data.Nominal_Voltage;
            Nominal_Voltage_Entry.Text = NomVolts.ToString();
            Manufacture_Name_Entry.Text = data.Manufacture_Name;
            Specifiaction_title_Entry.Text = data.Specifiaction_title;
            Specifiaction_NO_Entry.Text = data.Specifiaction_NO;
            Specifiaction_item_NO_Entry.Text = data.Specifiaction_item_NO;
            last_install_date_Entry.Text = data.last_install_date;
            Equipment_class_Entry.Text = data.Equipment_class;
            Equipment_class_description_Entry.Text = data.Equipment_class_description;
            addassetbutton.Text = "Update";
        }
        async void Add_Asset_Clicked(object sender, EventArgs e)
        {
            //if (!string.IsNullOrWhiteSpace(entry.Text) && !string.IsNullOrWhiteSpace(entry.Text))
            // {
            if (addassetbutton.Text == "Add Asset")
            {
                await App.Database.SaveStudentAsync(new Assets
                {
                    Substation_Code_selected = SubPicker.Items[SubPicker.SelectedIndex],
                    Plant_Number = Plant_Number_Entry.Text,
                    Asset_EQ_NO = Int32.Parse(Asset_EQ_NO_Entry.Text),
                    EQ_Status = EQ_Status_Entry.Text,
                    Serial_Number = Serial_Number_Entry.Text,
                    Modifier_code = Modifier_code_Entry.Text,
                    Location_Equipment_Number = Int32.Parse(Location_Equipment_Number_Entry.Text),
                    Component_Code = Component_Code_Entry.Text,
                    WarrantyDate = WarrantyDate_Picker.Date, // change dumb dumb. 
                    Equipement_age = Int32.Parse(Equipement_age_Entry.Text),
                    Stock_Code = Stock_Code_Entry.Text,
                    PO_NO = PO_NO_Entry.Text,
                    Rated_Voltage = Int32.Parse(Rated_Voltage_Entry.Text),
                    Nominal_Voltage = Int32.Parse(Nominal_Voltage_Entry.Text),
                    Manufacture_Name = Manufacture_Name_Entry.Text,
                    Specifiaction_title = Specifiaction_title_Entry.Text,
                    Specifiaction_NO = Specifiaction_NO_Entry.Text,
                    Specifiaction_item_NO = Specifiaction_item_NO_Entry.Text,
                    last_install_date = last_install_date_Entry.Text,
                    Equipment_class = Equipment_class_Entry.Text,
                    Equipment_class_description = Equipment_class_description_Entry.Text,
                });

            }
            else
            {
                await App.Database.UpdateStudentAsync(new Assets
                {

                    Id = Ids,
                    Substation_Code_selected = SubPicker.Items[SubPicker.SelectedIndex],
                    Plant_Number = Plant_Number_Entry.Text,
                    Asset_EQ_NO = Int32.Parse(Asset_EQ_NO_Entry.Text),
                    EQ_Status = EQ_Status_Entry.Text,
                    Serial_Number = Serial_Number_Entry.Text,
                    Modifier_code = Modifier_code_Entry.Text,
                    Location_Equipment_Number = Int32.Parse(Location_Equipment_Number_Entry.Text),
                    Component_Code = Component_Code_Entry.Text,
                    WarrantyDate = WarrantyDate_Picker.Date, // change dumb dumb. 
                    Equipement_age = Int32.Parse(Equipement_age_Entry.Text),
                    Stock_Code = Stock_Code_Entry.Text,
                    PO_NO = PO_NO_Entry.Text,
                    Rated_Voltage = Int32.Parse(Rated_Voltage_Entry.Text),
                    Nominal_Voltage = Int32.Parse(Nominal_Voltage_Entry.Text),
                    Manufacture_Name = Manufacture_Name_Entry.Text,
                    Specifiaction_title = Specifiaction_title_Entry.Text,
                    Specifiaction_NO = Specifiaction_NO_Entry.Text,
                    Specifiaction_item_NO = Specifiaction_item_NO_Entry.Text,
                    last_install_date = last_install_date_Entry.Text,
                    Equipment_class = Equipment_class_Entry.Text,
                    Equipment_class_description = Equipment_class_description_Entry.Text,
                }) ;
            }
                // }
                await Navigation.PushAsync(new View_Assets(null));
        }
        //else { await DisplayAlert("Incorrect", "Incorrect username or password", "Close");}
    }
}
