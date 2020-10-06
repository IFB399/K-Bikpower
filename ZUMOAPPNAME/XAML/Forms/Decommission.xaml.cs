using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Decommission : ContentPage
    {   
        AssetFormLinkManager afl_manager;
        AssetManager asset_manager;
        DecommissionManager decommission_manager;
        UserManager user_manager;
        SubstationManager substation_manager;

        DecommissionData decommissionForm = null;
        bool update = false;
        string subCode = null;
        ObservableCollection<Asset> globalAssets = new ObservableCollection<Asset>();

        public Decommission(DecommissionData savedForm = null, ObservableCollection<Asset> assets = null)
        {
            InitializeComponent();
            afl_manager = AssetFormLinkManager.DefaultManager;
            asset_manager = AssetManager.DefaultManager;
            decommission_manager = DecommissionManager.DefaultManager;
            user_manager = UserManager.DefaultManager;
            substation_manager = SubstationManager.DefaultManager;

            int count = 0;
            if (assets != null)
            {
                globalAssets = assets;
                count = assets.Count();
            }
            if (Device.RuntimePlatform == Device.UWP)
            {
                EmailCheck.IsVisible = true; //windows only feature for now
                EmailLabel.IsVisible = true;
            }
            ManageAssets_Button.Text = "Manage Assets (" + count.ToString() + ")";

            if (savedForm != null)
            {
                if (savedForm.MovedTo != "Project" || savedForm.MovedTo != "Scrap")
                {
                    subCode = savedForm.Location; //used to populate sub picker
                }
                if (savedForm.Id != null) //form has already been submitted
                {
                    decommissionForm = savedForm;
                    SubmitButton.Text = "Update Form";
                    update = true;
                }
                LoadForm(savedForm);
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Substation_Entry.ItemsSource = await substation_manager.GetAllSubCodes();
            if (subCode != null)
            {
                Substation_Entry.SelectedItem = subCode; //load substation code. Cannot be done in load function
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

            //SUBSTATION OR PROJECT NUMBER
            string location = null;
            if (Project_Button.IsChecked)
            {
                location = Project_Entry.Text; //check if valid first
            }
            else 
            {
                if (Substation_Entry.SelectedIndex != -1)
                {
                    location = Substation_Entry.SelectedItem.ToString();
                }
            }

                //SAVE NEW FORM
            if (update == false) //used to be if decommissionForm == null but didnt work
            {
                DecommissionData form = new DecommissionData
                {
                    DateDecommissioned = Date_Decommissioned.Date.ToLocalTime(),
                    Details = Decommissioned_Details_Entry.Text,
                    RegionName = regionName,
                    Location = location, 
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
                decommissionForm.Location = location;
                decommissionForm.MovedTo = movedto;
                decommissionForm.WorkOrderNumber = Work_OrderNo_Entry.Text;
                return null; //don't return a new form, just use the one that already exists
            }
        }
        private async void LoadForm(DecommissionData form)
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

            //load location
            //Substation_Entry.ItemsSource = await substation_manager.GetAllSubCodes();
            if (form.MovedTo == "Project")
            {
                if (form.Location != null)
                {
                    Project_Entry.Text = form.Location;
                }
            }

            //load work order number
            Work_OrderNo_Entry.Text = form.WorkOrderNumber;
        }

        async void Submit_Clicked(object sender, EventArgs e)
        {
            //VALIDATE INPUTS
            int intValue;
            bool condition1 = false; //must have at least one asset
            bool condition2 = false; //work order number must be an int
            bool condition3 = false; //moved to radio button must be selected
            bool condition4 = false; //project number must be provided and be an integer
            bool condition5 = false; //substation code must be provided (if it isnt project)
            if (globalAssets.Count() == 0 || globalAssets == null) 
            {
                condition1 = true;
            }
            if(!int.TryParse(Work_OrderNo_Entry.Text, out intValue)) 
            {
                condition2 = true;
            }
            if (Project_Button.IsChecked)
            {
                if (!int.TryParse(Project_Entry.Text, out intValue)) //project number must be an int
                {
                    condition4 = true; //not sure if this works if there is nothing in text box
                }
            }
            else
            {
                if (Substation_Entry.SelectedIndex == -1 && !Scrap_Button.IsChecked) //doesn't matter if it is scrapped
                {
                    condition5 = true; //substation must be provided
                }
            }

            if(!Project_Button.IsChecked && !Spares_Button.IsChecked && !Workshop_Button.IsChecked && !Scrap_Button.IsChecked)
            {
                condition3 = true;
            }
            if (condition1 || condition2 || condition3 || condition4 || condition5)
            {
                if (condition1)
                {
                    await DisplayAlert("Cannot Submit Form", "Please add at least one asset in the manage assets page", "Close");
                }
                else if (condition3)
                {
                    await DisplayAlert("Error", "Please select Project, Spares, Workshop or Scrap", "Close");
                }
                else if (condition2)
                {
                    await DisplayAlert("Error", "Please enter a valid work order number", "Close");
                }
                else if (condition4)
                {
                    await DisplayAlert("Error", "Please enter a valid project number", "Close");
                }
                else
                {
                    await DisplayAlert("Error", "Please provide a substation", "Close");
                }
            }

            else
            {
                DecommissionData form = SaveData();
                if (form != null) //form is being submitted for the first time
                {
                    bool answer = await DisplayAlert("Confirm Submission", "Submit this form?", "Yes", "No");
                    if (answer == true)
                    {
                        form.SubmittedBy = user_manager.ReturnName();
                        form.Status = "Submitted";
                        form.SubmittedOn = DateTime.UtcNow.ToLocalTime();
                        form.LastModifiedOn = DateTime.UtcNow.ToLocalTime();
                        await AddItem(form);
                        if (EmailCheck.IsChecked)
                        {
                            //SEND EMAIL!
                            string subject = "WARP TECH Approval Required";
                            string body = form.SubmittedBy + " has submitted a decommission form that is awaiting approval";
                            List<string> recipients = await user_manager.GetApproverEmails();
                            try
                            {
                                await SendEmail(subject, body, recipients);
                            }
                            catch
                            {
                                await DisplayAlert("Error", "Email failed", "Close");
                            }
                        }

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

                }
                else
                {
                    //update last modified on
                    decommissionForm.LastModifiedOn = DateTime.UtcNow.ToLocalTime();
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


        private void Project_Button_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Project_Button.IsChecked)
            {
                Project_Entry.IsVisible = true;
                Substation_Entry.IsEnabled = false;
            }
        }
        private void Spares_Button_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Spares_Button.IsChecked)
            {
                Project_Entry.IsVisible = false;
                Substation_Entry.IsEnabled = true;
            }
        }
        private void Workshop_Button_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Workshop_Button.IsChecked)
            {
                Project_Entry.IsVisible = false;
                Substation_Entry.IsEnabled = true;
            }
        }
        private void Scrap_Button_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Scrap_Button.IsChecked)
            {
                Project_Entry.IsVisible = false;
                Substation_Entry.IsEnabled = false;

            }

        }
        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                To = recipients,
            };
            await Email.ComposeAsync(message);

        }
    }
}