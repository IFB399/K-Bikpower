
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
    public partial class Commission : ContentPage
    {
        AssetFormLinkManager afl_manager;
        AssetManager asset_manager;
        CommissionManager commission_manager;
        UserManager user_manager;

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
                LoadForm(savedForm);
            }
        }
        async Task AddItem(CommissionData item)
        {
            await commission_manager.SaveTaskAsync(item);
        }
        async Task AddLink(AssetFormLink item)
        {
            await afl_manager.SaveTaskAsync(item);
        }
        private CommissionData SaveData()
        {
            string movedFrom = "";
            if (Project_Button.IsChecked)
            {
                movedFrom = Project_Button.Text;
            }
            if (Spares_Button.IsChecked)
            {
                movedFrom = Spares_Button.Text;
            }
            if (Workshop_Button.IsChecked)
            {
                movedFrom = Workshop_Button.Text;
            }

            string installation = "";
            if (Yes_Button1.IsChecked)
            {
                installation = Yes_Button1.Text;
            }
            if (No_Button1.IsChecked)
            {
                installation = No_Button1.Text;
            }

            string replacement = "";
            if (Yes_Button2.IsChecked)
            {
                replacement = Yes_Button2.Text;
            }
            if (No_Button2.IsChecked)
            {
                replacement = No_Button2.Text;
            }

            string regionName = null;
            if (Region_Picker.SelectedIndex != -1)
            {
                regionName = Region_Picker.SelectedItem.ToString();
            }
            int workOrderNumber = -1; //will have to change later, maybe store work order number as a string in the database
            if (String.IsNullOrEmpty(Work_OrderNo_Entry.Text) == false)
            {
                workOrderNumber = Int32.Parse(Work_OrderNo_Entry.Text); //will break if an int is not given
            }
            CommissionData form = new CommissionData
            {
                DateCommissioned = Date_Commissioned.Date.ToLocalTime(),
                NewInstallation = installation,
                Replacement = replacement,
                RegionName = regionName,
                Location = Location_Entry.Text,
                MovedFrom = movedFrom,
                WorkOrderNumber = workOrderNumber
            };
            return form;
        }
        private void LoadForm(CommissionData form)
        {
            //Date
            Date_Commissioned.Date = form.DateCommissioned;
            //installation
            if (form.NewInstallation == "Yes")
            {
                Yes_Button1.IsChecked = true;
            }
            else if (form.NewInstallation == "No")
            {
                No_Button1.IsChecked = true;
            }
            //replacement
            if (form.Replacement == "Yes")
            {
                Yes_Button2.IsChecked = true;
            }
            else if (form.Replacement == "No")
            {
                No_Button2.IsChecked = true;
            }
            //region
            if (form.RegionName != null)
            {
                Region_Picker.SelectedItem = form.RegionName;
            }
            //location
            Location_Entry.Text = form.Location;
            //movedFrom
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
            //work order number
            if (form.WorkOrderNumber != -1)
            {
                Work_OrderNo_Entry.Text = form.WorkOrderNumber.ToString();
            }
        }
        private void Scan_Asset_Clicked(object sender, EventArgs e)
        {
            CommissionData c = SaveData();
            Navigation.PushAsync(new ScanQR(c, globalAssets)); //go to scan page

        }
        async void Submit_Clicked(object sender, EventArgs e)
        {
            if (globalAssets.Count() == 0 || globalAssets == null)
            {
                await DisplayAlert("Cannot Submit Form", "Please add at least one asset in the manage assets page", "Close");
            }
            else
            {
                CommissionData form = SaveData();
                //form.SubmittedBy = user_manager.ReturnUser(); NEED TO FIX
                form.Status = "Submitted";

                await AddItem(form);
                await Navigation.PushAsync(new ViewCommissionForms());
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
                        await AddLink(afl);
                    }
                }
            }
        }
        private async void ManageAssets_Clicked(object sender, EventArgs e)
        {
            CommissionData c = SaveData();
            await Navigation.PushAsync(new ManageFormAssets(c, globalAssets)); //WILL FIX LATER

        }
    }
}