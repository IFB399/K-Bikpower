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
    public partial class Add_Sub : ContentPage
    {
        Substation_Codes SubData;
        public Add_Sub(Substation_Codes data)
        {
            InitializeComponent();
            if (data != null)
            {
                SubData = data;
                PopulateDetails(SubData);
            }
        }

        private void PopulateDetails(Substation_Codes data)
        {
            Substation_Code_Entry.Text = data.Substation_Code;
            Substation_Name_Entry.Text = data.SubstationName;
        }
        async void Add_Substation_Clicked(object sender, EventArgs e)
        {
            if (addsubbutton.Text == "Add Substation")
            {
                if (string.IsNullOrEmpty(Substation_Code_Entry.Text) || string.IsNullOrEmpty(Substation_Name_Entry.Text))
                {
                    await DisplayAlert("Alert", "Please ensure all fields are complete", "OK");
                }
                else
                {
                    await App.Database.SaveSubAsync(new Substation_Codes
                    {
                        Substation_Code = Substation_Code_Entry.Text,
                        SubstationName = Substation_Name_Entry.Text
                    });
                    await Navigation.PushAsync(new Substations());
                }
            }
            else //update?
            {
                if (string.IsNullOrEmpty(Substation_Code_Entry.Text) || string.IsNullOrEmpty(Substation_Name_Entry.Text))
                {
                    await DisplayAlert("Alert", "Please ensure all fields are complete", "OK");
                }
                else
                {
                    await App.Database.UpdateSubAsync(new Substation_Codes
                    {
                        Substation_Code = Substation_Code_Entry.Text,
                        SubstationName = Substation_Name_Entry.Text
                    });
                    await Navigation.PushAsync(new Substations());
                }
            }
            

        }
    }
}