
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Windows.ApplicationModel.Store.Preview.InstallControl;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        CommissionData commissionForm = null;
        bool update = false;
        ObservableCollection<Asset> globalAssets = new ObservableCollection<Asset>();
        public Commission(CommissionData savedForm = null, ObservableCollection<Asset> assets = null)
        {
            InitializeComponent();

            afl_manager = AssetFormLinkManager.DefaultManager;
            asset_manager = AssetManager.DefaultManager;
            commission_manager = CommissionManager.DefaultManager;
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
                commissionForm = savedForm;
                if (savedForm.Id != null) //form has already been submitted
                {
                    SubmitButton.Text = "Update Form";
                    update = true;
                }
                LoadForm(savedForm);
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
            /*
            //WORK ORDER NUMBER
            int workOrderNumber = -1; //will have to change later, maybe store work order number as a string in the database
            if (String.IsNullOrEmpty(Work_OrderNo_Entry.Text) == false)
            {
                workOrderNumber = Int32.Parse(Work_OrderNo_Entry.Text); //will break if an int is not given
            }
            */
            //SAVE NEW FORM
            if (update == false) //used to be if commissionForm == null but didnt work
            {
                CommissionData form = new CommissionData
                {
                    DateCommissioned = Date_Commissioned.Date.ToLocalTime(),
                    NewInstallation = installation,
                    Replacement = replacement,
                    RegionName = regionName,
                    Location = Location_Entry.Text,
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
                commissionForm.Location = Location_Entry.Text;
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
            Location_Entry.Text = form.Location;

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
            /*
            if (form.WorkOrderNumber != -1)
            {
                Work_OrderNo_Entry.Text = form.WorkOrderNumber.ToString();
            }
            */
        }
        async void Submit_Clicked(object sender, EventArgs e)
        {
            if (globalAssets.Count() == 0 || globalAssets == null)
            {
                await DisplayAlert("Cannot Submit Form", "Please add at least one asset in the manage assets page", "Close");
            }
            else
            {
                CommissionData form = SaveData(); //returns null or a new form
                if (form != null) //form is being submitted for the first time
                {
                    form.SubmittedBy = user_manager.ReturnName(); //add submitted by name
                    form.Status = "Submitted"; //make status of form submitted
                    form.SubmittedOn = DateTime.UtcNow;
                    form.LastModifiedOn = DateTime.UtcNow;
                    await AddItem(form); //add form to database
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
                else //existing form is being updated
                {
                    //change a modified by field?
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
    }
}