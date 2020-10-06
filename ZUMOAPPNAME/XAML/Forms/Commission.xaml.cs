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
    public partial class Commission : ContentPage
    {
        //managers
        AssetFormLinkManager afl_manager;
        AssetManager asset_manager;
        CommissionManager commission_manager;
        UserManager user_manager;
        SubstationManager substation_manager;

        CommissionData commissionForm = null;
        bool update = false;
        string subCode = null;
        ObservableCollection<Asset> globalAssets = new ObservableCollection<Asset>();
        public Commission(CommissionData savedForm = null, ObservableCollection<Asset> assets = null)
        {
            InitializeComponent();

            afl_manager = AssetFormLinkManager.DefaultManager;
            asset_manager = AssetManager.DefaultManager;
            commission_manager = CommissionManager.DefaultManager;
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
                if (savedForm.MovedFrom != "Project")
                {
                    subCode = savedForm.Location; //used to populate sub picker
                }
                commissionForm = savedForm;
                if (savedForm.Id != null) //form has already been submitted
                {
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
        async Task AddItem(CommissionData item)
        {
            await commission_manager.SaveTaskAsync(item); //adds commission form to database or updates an existing one
        }
        async Task AddLink(AssetFormLink item)
        {
            await afl_manager.SaveTaskAsync(item);
        }
        private CommissionData SaveData()
        {
            //MOVED FROM RADIO BUTTON
            string movedFrom = "";
            if (Project_Button.IsChecked)
            {
                movedFrom = Project_Button.Text;
            }
            else if (Spares_Button.IsChecked)
            {
                movedFrom = Spares_Button.Text;
            }
            else if (Workshop_Button.IsChecked)
            {
                movedFrom = Workshop_Button.Text;
            }

            //INSTALLATION CHECK BOX
            string installation = "";
            if (InstallationYes.IsChecked)
            {
                installation = "Yes";
            }
            else if (InstallationNo.IsChecked)
            {
                installation = "No";
            }

            //REPLACEMENT CHECK BOX
            string replacement = "";
            if (ReplacementYes.IsChecked)
            {
                replacement = "Yes";
            }
            else if (ReplacementNo.IsChecked)
            {
                replacement = "No";
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
            if (update == false) //used to be if commissionForm == null but didnt work
            {
                CommissionData form = new CommissionData
                {
                    DateCommissioned = Date_Commissioned.Date.ToLocalTime(),
                    NewInstallation = installation,
                    Replacement = replacement,
                    RegionName = regionName,
                    Location = location,
                    MovedFrom = movedFrom,
                    WorkOrderNumber = Work_OrderNo_Entry.Text
                };
                return form; //brand new form
            }
            //UPDATE EXISTING FORM
            else
            {
                commissionForm.DateCommissioned = Date_Commissioned.Date.ToLocalTime();
                commissionForm.NewInstallation = installation;
                commissionForm.Replacement = replacement;
                commissionForm.RegionName = regionName;
                commissionForm.Location = location;
                commissionForm.MovedFrom = movedFrom;
                commissionForm.WorkOrderNumber = Work_OrderNo_Entry.Text;
                return null; //don't return a new form, just use the one that already exists
            }
            
        }
        private void LoadForm(CommissionData form)
        {
            
            //Load date
            Date_Commissioned.Date = form.DateCommissioned;

            //load replacement
            if (form.Replacement == "Yes")
            {
                ReplacementYes.IsChecked = true;
            }
            else if (form.Replacement == "No")
            {
                ReplacementNo.IsChecked = true;
            }

            //load installation
            if (form.NewInstallation == "Yes")
            {
                InstallationYes.IsChecked = true;
            }
            else if (form.NewInstallation == "No")
            {
                InstallationNo.IsChecked = true;
            }

            //load region
            if (form.RegionName != null)
            {
                Region_Picker.SelectedItem = form.RegionName;
            }

            //load location
            //Location_Entry.Text = form.Location;
            //Substation_Entry.ItemsSource = await substation_manager.GetAllSubCodes();
            if (form.MovedFrom == "Project")
            {
                if (form.Location != null)
                {
                    Project_Entry.Text = form.Location;
                }
            }

            //load movedFrom
            if (form.MovedFrom == "Project")
            {
                Project_Button.IsChecked = true;
            }
            else if (form.MovedFrom == "Spares")
            {
                Spares_Button.IsChecked = true;
            }
            else if (form.MovedFrom == "Workshop")
            {
                Workshop_Button.IsChecked = true;
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
            if (globalAssets.Count() == 0 || globalAssets == null) //must have at least one asset
            {
                condition1 = true;
            }
            if (!int.TryParse(Work_OrderNo_Entry.Text, out intValue)) //work order number must be an int
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
                if (Substation_Entry.SelectedIndex == -1)
                {
                    condition5 = true; //substation must be provided
                }
            }
            if ((!Project_Button.IsChecked && !Spares_Button.IsChecked && !Workshop_Button.IsChecked)
                || (!InstallationYes.IsChecked && !InstallationNo.IsChecked)
                || (!ReplacementNo.IsChecked && !ReplacementYes.IsChecked))
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
                    await DisplayAlert("Error", "Please fill all required fields", "Close");
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
                CommissionData form = SaveData(); //returns null or a new form
                if (form != null) //form is being submitted for the first time
                {
                    bool answer = await DisplayAlert("Confirm Submission", "Submit this form?", "Yes", "No");
                    if (answer == true)
                    {
                        form.SubmittedBy = user_manager.ReturnName(); //add submitted by name
                        form.Status = "Submitted"; //make status of form submitted
                        form.SubmittedOn = DateTime.UtcNow.ToLocalTime();
                        form.LastModifiedOn = DateTime.UtcNow.ToLocalTime();
                        await AddItem(form); //add form to database
                        if (EmailCheck.IsChecked)
                        {
                            //SEND EMAIL!
                            string subject = "WARP TECH Approval Required";
                            string body = form.SubmittedBy + " has submitted a commission form that is awaiting approval";
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
                                    FormType = "Commission"
                                };
                                await AddLink(afl); //add a link to database 
                            }
                        }
                    }

                }
                else //existing form is being updated
                {
                    //update last modified on
                    commissionForm.LastModifiedOn = DateTime.UtcNow.ToLocalTime(); 
                    //delete old links
                    ObservableCollection<AssetFormLink> afls = await afl_manager.GetLinksByFormAsync(commissionForm.Id, "Commission");
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
                            FormId = commissionForm.Id, //use existing form id
                            AssetId = a.Id,
                            FormType = "Commission"
                        };
                        await AddLink(afl);
                    }

                    await AddItem(commissionForm); //SHOULD UPDATE EXISTING FORM
                }

                await Navigation.PushAsync(new ViewCommissionForms()); //return to view commission page

            }
        }
        private async void ManageAssets_Clicked(object sender, EventArgs e)
        {
            //update existing instance or return a new form
            CommissionData c = SaveData(); 
            if (c != null) //new form
            {
                await Navigation.PushAsync(new ManageFormAssets(c, globalAssets));
            }
            else
            {
                await Navigation.PushAsync(new ManageFormAssets(commissionForm, globalAssets)); //send existing instance
            }

        }

        //Making check boxes work like radio buttons
        private void InstallationYes_changed(object sender, EventArgs e)
        {
            if (InstallationYes.IsChecked)
            {
                InstallationNo.IsChecked = false;
            }
        }
        private void InstallationNo_changed(object sender, EventArgs e)
        {
            if (InstallationNo.IsChecked)
            {
                InstallationYes.IsChecked = false;
            }
        }

        private void ReplacementYes_changed(object sender, EventArgs e)
        {
            if (ReplacementYes.IsChecked)
            {
                ReplacementNo.IsChecked = false;
            }
        }

        private void ReplacementNo_changed(object sender, EventArgs e)
        {
            if (ReplacementNo.IsChecked)
            {
                ReplacementYes.IsChecked = false;
            }
        }

        private void Project_Button_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Project_Button.IsChecked)
            {
                Project_Entry.IsVisible = true;
                Substation_Entry.IsEnabled = false;
            }
            else
            {
                Project_Entry.IsVisible = false;
                Substation_Entry.IsEnabled = true;
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