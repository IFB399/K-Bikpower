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
        TableManagerSub subtable;
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

        async Task AddItem(Substation_Codes item)
        {
            await subtable.SaveTaskAsync(item);
        }
        async void Button_Clicked(object sender, EventArgs e)
        {
            if (addsubbutton.Text == "Add Asset")
            {
                var sub = new Substation_Codes
                {
                    Substation_Code = Substation_Code_Entry.Text,
                    Substation_Name = Substation_Name_Entry.Text
                };
                await AddItem(sub);

            }
            else 
            {
                var sub = new Substation_Codes
                {
                    Substation_Code = Substation_Code_Entry.Text,
                    Substation_Name = Substation_Name_Entry.Text
                };
                await AddItem(sub);
            }
            await Navigation.PushAsync(new Substations());

        }
    }
}