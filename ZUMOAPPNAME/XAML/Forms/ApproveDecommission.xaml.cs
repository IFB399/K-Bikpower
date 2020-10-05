﻿using System;
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

            //Debug.Text = user_manager.Authentication();

            decommission_form = submittedForm;
            if (assets != null)
            {
                globalAssets = assets;

            }
            RetrieveAssets(submittedForm, assets);
            LoadForm(submittedForm);

            assetList.ItemsSource = globalAssets;

            if (submittedForm.Status == "Approved") //the form can only be viewed
            {
                DeleteButton.IsEnabled = false; //can't delete a approved form
                EditButton.IsEnabled = false; //can't edit a approved form
                RejectButton.IsEnabled = false; //can't reject a approved form?
                ApproveButton.IsEnabled = false; //can't approve an already approved form
            }
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
                ExpanderLabel.Text = "View Assets (" + count.ToString() + ")";
            }
            else //count the assets that have already been retrieved
            {
                count = globalAssets.Count();
                ExpanderLabel.Text = "View Assets (" + count.ToString() + ")";
            }
        }
        private void LoadForm(DecommissionData form)
        {
            SubmittedByLabel.Text = form.SubmittedBy;
            SubmittedOnLabel.Text = form.SubmittedOn.ToString("d");
            DateDecommissionedLabel.Text = form.DateDecommissioned.ToString("d");
            DetailsLabel.Text = form.Details;
            RegionLabel.Text = form.RegionName;
            LocationLabel.Text = form.Location;
            MovedToLabel.Text = form.MovedTo;
            WorkLabel.Text = form.WorkOrderNumber.ToString();
            
        }

        async void Approve_Clicked(object sender, EventArgs e)
        {
            string role = user_manager.Authentication();
            if (role == "Chief Operating Officer" || role == "Regional Maintenance" || role == "Asset Strategy Manager"
                || role == "Executive Manager Projects" || role == "Major Capital Projects Manager")
            {
                bool answer = await DisplayAlert("Confirm Approval", "Approve this form?", "Yes", "No");
                if (answer == true)
                {
                    //decommission_form.Status = "Approved by " + user_manager.ReturnUser();
                    decommission_form.ApprovedBy = user_manager.ReturnName();
                    decommission_form.LastModifiedOn = DateTime.UtcNow.ToLocalTime();
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
            else
            {
                await DisplayAlert("Error", "You do not have permission to approve this form", "Close");
            }
            

        }
        async void Reject_Clicked(object sender, EventArgs e)
        {
            string role = user_manager.Authentication();
            if (role == "Chief Operating Officer" || role == "Regional Maintenance" || role == "Asset Strategy Manager"
                || role == "Executive Manager Projects" || role == "Major Capital Projects Manager")
            {
                bool answer = await DisplayAlert("Confirm Rejection", "Reject this form?", "Yes", "No");
                if (answer == true)
                {
                    decommission_form.RejectedBy = user_manager.ReturnName();
                    decommission_form.LastModifiedOn = DateTime.UtcNow.ToLocalTime();
                    decommission_form.Status = "Rejected";
                    await UpdateForm(decommission_form);
                    await Navigation.PushAsync(new ViewDecommissionForms());
                }
            }
            else
            {
                await DisplayAlert("Error", "You do not have permission to reject this form", "Close");
            }
        }
        async void Edit_Clicked(object sender, EventArgs e)
        {
            //SHOULD ONLY BE POSSIBLE IF NOT APPROVED
            //can only be done by person who submitted or someone who is able to approve/reject?
            await Navigation.PushAsync(new Decommission(decommission_form, globalAssets));
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
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (this.assetList.SelectedItem != null) //make sure asset isnt selected
            {
                this.assetList.SelectedItem = null;
            }
        }
        private async void selectedAsset(object sender, EventArgs e)
        {
            if (((ListView)sender).SelectedItem == null)
                return;
            await Navigation.PushAsync(new FormPreviewAsset((Asset)assetList.SelectedItem, 5, decommission_form, globalAssets));
        }
    }
}
