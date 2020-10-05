using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Add_Sub : ContentPage
    {
        Substation SubData;
        SubstationManager manager;
        UserManager user_manager; //WILL REMOVE LATER
        public Add_Sub(Substation data)
        {
            InitializeComponent();
            manager = SubstationManager.DefaultManager;
            user_manager = UserManager.DefaultManager;
            if (data != null)
            {
                SubData = data;
                PopulateDetails(SubData);
            }
        }

        private void PopulateDetails(Substation data)
        {
            Substation_Code_Entry.Text = data.Substation_Code;
            Substation_Name_Entry.Text = data.Substation_Name;
            Substation_Area_Entry.Text = data.Area;
        }

        async Task AddItem(Substation item)
        {
            await manager.SaveTaskAsync(item);
        }
        async void Button_Clicked(object sender, EventArgs e)
        {
            if (addsubbutton.Text == "Add Substation")
            {
                bool condition1 = false; //substation code must be filled
                bool condition2 = false; //substation code must be unique

                if (string.IsNullOrEmpty(Substation_Code_Entry.Text))
                {
                    condition1 = true;
                }
                else //only check uniqueness if actually filled
                {   
                    ObservableCollection<Substation> s = await manager.GetSubstationByCode(Substation_Code_Entry.Text);
                    
                    
                    if (s.Any()) //already existing substation was found
                    {
                        condition2 = true;
                    }
                    
                }
                if (condition1 || condition2)
                {
                    if (condition1)
                    {
                        await DisplayAlert("Error", "Please provide a substation code", "Close");
                    }
                    else
                    {
                        string sentence = "Substation with code " + Substation_Code_Entry.Text + " already exists";
                        await DisplayAlert("Error", sentence, "Close");
                    }
                }
                else
                {
                    var sub = new Substation
                    {
                        Substation_Code = Substation_Code_Entry.Text,
                        Substation_Name = Substation_Name_Entry.Text,
                        Area = Substation_Area_Entry.Text
                    };
                    await AddItem(sub);
                    await Navigation.PushAsync(new ViewSubstations());
                }
            }
            else
            {
                //HAVEN'T DONE UPDATE SUBSTATION YET
                var sub = new Substation
                {
                    Substation_Code = Substation_Code_Entry.Text,
                    Substation_Name = Substation_Name_Entry.Text,
                    Area = Substation_Area_Entry.Text
                    //Area = "areaaa"
                };
                await AddItem(sub);
            }
            //await Navigation.PushAsync(new ViewSubstations());

        }

    }
}