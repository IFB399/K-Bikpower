using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApproveDecommission : ContentPage
    {
        AssetFormLinkManager afl_manager;
        AssetManager asset_manager;
        DecommissionManager decommission_manager;
        UserManager user_manager;

        DecommissionData decommission_form;
        ObservableCollection<Asset> globalAssets = new ObservableCollection<Asset>();

        public ApproveDecommission(DecommissionData submittedForm, ObservableCollection<Asset> assets = null)
        {
            InitializeComponent();
            afl_manager = AssetFormLinkManager.DefaultManager;
            asset_manager = AssetManager.DefaultManager;
            decommission_manager = DecommissionManager.DefaultManager;
            user_manager = UserManager.DefaultManager;

            decommission_form = submittedForm;
            if (submittedForm.Status.Contains("Approved") == false)
            {
                DeleteButton.IsEnabled = true;
            }

            if (assets != null)
            {
                globalAssets = assets;
                
            }
            RetrieveAssets(submittedForm, assets);
            LoadForm(submittedForm);
        }

        async Task UpdateAsset(Asset item)
        {
            await asset_manager.SaveTaskAsync(item);
        }
        async Task UpdateForm(DecommissionData form)
        {
            await decommission_manager.SaveTaskAsync(form);
        }
        private async void RetrieveAssets(DecommissionData form, ObservableCollection<Asset> assets)
        {
            int count = 0;
            if (assets == null)
            {
                ObservableCollection<string> assetIds = await afl_manager.GetAssetsAsync(form.Id,"Decommission");
                foreach (string s in assetIds)
                {
                    ObservableCollection<Asset> a = await asset_manager.GetAsset(s);
                    Asset asset = a.FirstOrDefault();
                    globalAssets.Add(asset);
                }
                count = globalAssets.Count();
                ManageAssets_Button.Text = "View Assets (" + count.ToString() + ")";
            }
            else //count the assets that have already been retrieved
            {
                count = globalAssets.Count();
                ManageAssets_Button.Text = "View Assets (" + count.ToString() + ")";
            }
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

        async void Approve_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirm Approval", "Approve this form?", "Yes", "No");
            if (answer == true)
            {
                //decommission_form.Status = "Approved by " + user_manager.ReturnUser();
                decommission_form.Status = "Approved";
                await UpdateForm(decommission_form);
                //update asset form links?
                await Navigation.PushAsync(new ViewDecommissionForms());
                //change status of assets (only happends after approval)
                foreach (Asset a in globalAssets)
                {
                    a.Status = "Decommissioned";
                    await UpdateAsset(a);
                }
            }

        }
        async void Reject_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirm Rejection", "Reject this form?", "Yes", "No");
            if (answer == true)
            {
                //decommission_form.Status = "Rejected by " + user_manager.ReturnUser();
                decommission_form.Status = "Rejected";
                await UpdateForm(decommission_form);
                await Navigation.PushAsync(new ViewDecommissionForms());
            }
        }
        async void Exit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewDecommissionForms());
        }
        async void Delete_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirm Deletion", "Delete this form?", "Yes", "No");
            if (answer == true)
            {
                //delete form and afls
                ObservableCollection<AssetFormLink> afls = await afl_manager.GetLinksByFormAsync(decommission_form.Id, "Decommission");
                foreach (AssetFormLink afl in afls)
                {
                    await afl_manager.DeleteLinkAsync(afl); //delete all the links
                }
                await decommission_manager.DeleteFormAsync(decommission_form);
                await Navigation.PushAsync(new ViewDecommissionForms());
            }


        }
        private async void ManageAssets_Clicked(object sender, EventArgs e)
        {
            //DecommissionData d = SaveData();
            await Navigation.PushAsync(new ManageFormAssets(decommission_form, globalAssets, true)); //indicate that we have come from approval page

        }
    }
}
