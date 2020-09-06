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
        Substations SubData;
      //  TableManagerSub subtable;
        public Add_Sub(Substations data)
        {
            InitializeComponent();
            if (data != null)
            {
                SubData = data;
                PopulateDetails(SubData);
            }
        }

        private void PopulateDetails(Substations data)
        {
            //Substation_Code_Entry.Text = data.SubstationCode;
//Substation_Name_Entry.Text = data.SubstationName;
        }

        async Task AddItem(Substations item)
        {
            //await subtable.SaveTaskAsync(item);
        }
        async void Button_Clicked(object sender, EventArgs e)
        {
            if (addsubbutton.Text == "Add Asset")
            {
                var sub = new Substations
                {
                 //   SubstationCode = Substation_Code_Entry.Text,
                  //  SubstationName = Substation_Name_Entry.Text
                };
                await AddItem(sub);

            }
            else 
            {
                var sub = new Substations
                {
                    //SubstationCode = Substation_Code_Entry.Text,
                   // SubstationName = Substation_Name_Entry.Text
                };
                await AddItem(sub);
            }
            await Navigation.PushAsync(new ViewSubstations());

        }
    }
}