using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Decommission : ContentPage
    {   
        AssetFormLinkManager afl_manager;
        AssetManager asset_manager;
        DecommissionManager decommission_manager;
        UserManager user_manager;

        DecommissionData decommissionForm = null;
        bool update = false;
        ObservableCollection<Asset> globalAssets = new ObservableCollection<Asset>();

        public Decommission(DecommissionData savedForm = null, ObservableCollection<Asset> assets = null)
        {
            InitializeComponent();
            afl_manager = AssetFormLinkManager.DefaultManager;
            asset_manager = AssetManager.DefaultManager;
            decommission_manager = DecommissionManager.DefaultManager;
            user_manager = UserManager.DefaultManager;

            dateLabel.Text = DateTime.UtcNow.ToString("d");

            int count = 0;
            if (assets != null)
            {
                globalAssets = assets;
                count = assets.Count();
            }
            ManageAssets_Button.Text = "Manage Assets (" + count.ToString() + ")";

            if (savedForm != null)
            {
                if (savedForm.Id != null) //form has already been submitted
                {
                    decommissionForm = savedForm;
                    SubmitButton.Text = "Update Form";
                    update = true;
                }
                LoadForm(savedForm);
            }
        }
        async Task AddItem(DecommissionData item)
        {
            await decommission_manager.SaveTaskAsync(item); //adds decommission form to database or updates an existing one
        }
        async Task AddLink(AssetFormLink item)
        {
            await afl_manager.SaveTaskAsync(item);
        }
        private DecommissionData SaveData()
        {
            //MOVED TO RADIO BUTTON
            string movedto = "";
            if (Scrap_Button.IsChecked)
            {
                movedto = Scrap_Button.Text;
            }
            if (Project_Button.IsChecked)
            {
                movedto = Project_Button.Text;
            }
            if (Spares_Button.IsChecked)
            {
                movedto = Spares_Button.Text;
            }
            if (Workshop_Button.IsChecked)
            {
                movedto = Workshop_Button.Text;
            }

            //REGION PICKER
            string regionName = null;
            if (Region_Picker.SelectedIndex != -1)
            {
                regionName = Region_Picker.SelectedItem.ToString();
            }
            /*
            //WORK ORDER NUMBER
            int workOrderNumber = -1; //will have to change later, maybe store work order number as a string in the database
            if (String.IsNullOrEmpty(Work_OrderNo_Entry.Text) == false)
            {
                workOrderNumber = Int32.Parse(Work_OrderNo_Entry.Text); //will break if an int is not given
            }
            */
            //SAVE NEW FORM
            if (update == false) //used to be if decommissionForm == null but didnt work
            {
                DecommissionData form = new DecommissionData
                {
                    DateDecommissioned = Date_Decommissioned.Date.ToLocalTime(),
                    Details = Decommissioned_Details_Entry.Text,
                    RegionName = regionName,
                    Location = Location_Entry.Text,
                    MovedTo = movedto,
                    WorkOrderNumber = Work_OrderNo_Entry.Text
                };
                return form; //brand new form
            }
            //UPDATE EXISTING FORM
            else
            {
                decommissionForm.DateDecommissioned = Date_Decommissioned.Date.ToLocalTime();
                decommissionForm.Details = Decommissioned_Details_Entry.Text;
                decommissionForm.RegionName = regionName;
                decommissionForm.Location = Location_Entry.Text;
                decommissionForm.MovedTo = movedto;
                decommissionForm.WorkOrderNumber = Work_OrderNo_Entry.Text;
                return null; //don't return a new form, just use the one that already exists
            }
        }
        private void LoadForm(DecommissionData form)
        {
            //load date
            Date_Decommissioned.Date = form.DateDecommissioned;

            //load details
            Decommissioned_Details_Entry.Text = form.Details;

            //load region
            if (form.RegionName != null)
            {
                Region_Picker.SelectedItem = form.RegionName;
            }

            //load location
            Location_Entry.Text = form.Location;

            //load moved to
            if (form.MovedTo == "Scrap")
            {
                Scrap_Button.IsChecked = true;
            }
            else if (form.MovedTo == "Project")
            {
                Project_Button.IsChecked = true;
            }
            else if (form.MovedTo == "Spares")
            {
                Spares_Button.IsChecked = true;
            }
            else if (form.MovedTo == "Workshop")
            {
                Workshop_Button.IsChecked = true;
            }
            /*
            //load work order number
            if (form.WorkOrderNumber != -1)
            {
                Work_OrderNo_Entry.Text = form.WorkOrderNumber.ToString();
            }
            */
            Work_OrderNo_Entry.Text = form.WorkOrderNumber;
        }

        async void Submit_Clicked(object sender, EventArgs e)
        {
            //VALIDATE INPUTS
            int intValue;
            bool condition1 = false;
            bool condition2 = false;
            bool condition3 = false;
            if (globalAssets.Count() == 0 || globalAssets == null) //must have at least one asset
            {
                condition1 = true;
            }
            if(!int.TryParse(Work_OrderNo_Entry.Text, out intValue)) //work order number must be an int
            {
                condition2 = true;
            }
            if((!Project_Button.IsChecked && !Spares_Button.IsChecked && !Workshop_Button.IsChecked && !Scrap_Button.IsChecked) 
                || string.IsNullOrEmpty(Location_Entry.Text))
            {
                condition3 = true;
            }
            if (condition1 || condition2 || condition3)
            {
                if (condition1)
                {
                    await DisplayAlert("Cannot Submit Form", "Please add at least one asset in the manage assets page", "Close");
                }
                else if (condition3)
                {
                    await DisplayAlert("Error", "Please fill all required fields", "Close");
                }
                else
                {
                    await DisplayAlert("Error", "Please enter a valid work order number", "Close");
                }
            }

            else
            {
                DecommissionData form = SaveData();
                if (form != null) //form is being submitted for the first time
                {
                    form.SubmittedBy = user_manager.ReturnName();
                    form.Status = "Submitted";
                    form.SubmittedOn = DateTime.UtcNow;
                    form.LastModifiedOn = DateTime.UtcNow;
                    await AddItem(form);
                    if (globalAssets != null)
                    {
                        foreach (Asset a in globalAssets)
                        {
                            //to ensure assets of this form can be retrieved
                            AssetFormLink afl = new AssetFormLink
                            {
                                FormId = form.Id,
                                AssetId = a.Id,
                                FormType = "Decommission"
                            };
                            await AddLink(afl);
                        }
                    }
                }
                else
                {
                    //change a modified by field?
                    //delete old links
                    ObservableCollection<AssetFormLink> afls = await afl_manager.GetLinksByFormAsync(decommissionForm.Id, "Decommission");
                    foreach (AssetFormLink afl in afls)
                    {
                        await afl_manager.DeleteLinkAsync(afl); //delete all the links
                    }
                    //add links again
                    foreach (Asset a in globalAssets)
                    {
                        //to ensure assets of this form can be retrieved
                        AssetFormLink afl = new AssetFormLink
                        {
                            FormId = decommissionForm.Id, //use existing form id
                            AssetId = a.Id,
                            FormType = "Decommission"
                        };
                        await AddLink(afl);
                    }

                    await AddItem(decommissionForm); //SHOULD UPDATE EXISTING FORM
                }
                await Navigation.PushAsync(new ViewDecommissionForms());
            }
        }
        private async void ManageAssets_Clicked(object sender, EventArgs e)
        {
            //update existing instance or return a new form
            DecommissionData d = SaveData();
            if (d != null) //new form
            {
                await Navigation.PushAsync(new ManageFormAssets(d, globalAssets));
            }
            else
            {
                await Navigation.PushAsync(new ManageFormAssets(decommissionForm, globalAssets)); //send existing instance
            }
        }
    }
}