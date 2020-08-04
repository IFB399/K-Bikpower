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
            Substation_Name_Entry.Text = data.Substation_Name;
        }
        async void Button_Clicked(object sender, EventArgs e)
        {
            if (addsubbutton.Text == "Add Asset")
            {
                await App.Database.SaveSubAsync(new Substation_Codes
                {
                    Substation_Code = Substation_Code_Entry.Text,
                    Substation_Name = Substation_Name_Entry.Text
                });
            }
            else 
            {
                await App.Database.UpdateSubAsync(new Substation_Codes
                {
                    Substation_Code = Substation_Code_Entry.Text,
                    Substation_Name = Substation_Name_Entry.Text
                });
            }
            Navigation.PushAsync(new Substations());

        }
    }
}