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
                LoadForm(savedForm);
            }

        }
        async Task AddItem(DecommissionData item)
        {
            await decommission_manager.SaveTaskAsync(item);
        }
        async Task AddLink(AssetFormLink item)
        {
            await afl_manager.SaveTaskAsync(item);
        }
        async Task UpdateAsset(Asset item)
        {
            await asset_manager.SaveTaskAsync(item);
        }

        private DecommissionData SaveData()
        {
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
            DecommissionData form = new DecommissionData
            {
                DateDecommissioned = Date_Decommissioned.Date.ToLocalTime(),
                Details = Decommissioned_Details_Entry.Text,
                RegionName = regionName,
                Location = Location_Entry.Text,
                MovedTo = movedto,
                WorkOrderNumber = workOrderNumber               
            };
            return form;
        }
        private void LoadForm(DecommissionData form)
        {
            Date_Decommissioned.Date = form.DateDecommissioned;
            Decommissioned_Details_Entry.Text = form.Details;
            if (form.RegionName != null)
            {
                Region_Picker.SelectedItem = form.RegionName;
            }
            Location_Entry.Text = form.Location;
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
            if (form.WorkOrderNumber != -1)
            {
                Work_OrderNo_Entry.Text = form.WorkOrderNumber.ToString();
            }
        }

        private void Scan_Asset_Clicked(object sender, EventArgs e)
        {
            DecommissionData d = SaveData();
            Navigation.PushAsync(new ScanQR(d, globalAssets)); //go to scan page

        }
        async void Submit_Clicked(object sender, EventArgs e)
        {
            if (globalAssets.Count() == 0 || globalAssets == null)
            {
                await DisplayAlert("Cannot Submit Form", "Please add at least one asset in the manage assets page", "Close");
            }
            else
            {
                DecommissionData form = SaveData();
                form.SubmittedBy = user_manager.ReturnUser();
                form.Status = "Submitted";

                await AddItem(form);
                await Navigation.PushAsync(new ViewDecommissionForms());
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
        private async void ManageAssets_Clicked(object sender, EventArgs e)
        {
            DecommissionData d = SaveData();
            await Navigation.PushAsync(new ManageFormAssets(d, globalAssets));

        }
    }
}