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
    public partial class ApproveCommission : ContentPage
    {
        AssetFormLinkManager afl_manager;
        AssetManager asset_manager;
        CommissionManager commission_manager;
        UserManager user_manager;
        ObservableCollection<Asset> globalAssets = new ObservableCollection<Asset>();
        CommissionData commission_form;

        public ApproveCommission(CommissionData submittedForm, ObservableCollection<Asset> assets = null)
        {
            InitializeComponent();

            afl_manager = AssetFormLinkManager.DefaultManager;
            asset_manager = AssetManager.DefaultManager;
            commission_manager = CommissionManager.DefaultManager;
            user_manager = UserManager.DefaultManager;
            commission_form = submittedForm;
            if (assets != null)
            {
                globalAssets = assets;
            }
            RetrieveAssets(submittedForm, assets); //will add assets to global assets
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
        async Task UpdateForm(CommissionData form)
        {
            await commission_manager.SaveTaskAsync(form);
        }
        private async void RetrieveAssets(CommissionData form, ObservableCollection<Asset> assets)
        {
            int count = 0;
            if (assets == null)
            {
                ObservableCollection<string> assetIds = await afl_manager.GetAssetsAsync(form.Id,"Commission");
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
        private void LoadForm(CommissionData form)
        {
            SubmittedByLabel.Text = form.SubmittedBy;
            DateCommissionedLabel.Text = form.DateCommissioned.ToString("d");
            InstallationLabel.Text = form.NewInstallation;
            ReplacementLabel.Text = form.Replacement;
            RegionLabel.Text = form.RegionName;
            LocationLabel.Text = form.Location;
            MovedFromLabel.Text = form.MovedFrom;
            WorkLabel.Text = form.WorkOrderNumber.ToString();
        }

        async void Approve_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirm Approval", "Approve this form?", "Yes", "No");
            if (answer == true)
            {
                //decommission_form.Status = "Approved by " + user_manager.ReturnUser();
                commission_form.ApprovedBy = user_manager.ReturnName();
                commission_form.Status = "Approved";
                await UpdateForm(commission_form);
                //update asset form links?
                await Navigation.PushAsync(new ViewCommissionForms());
                //change status of assets (only happends after approval)
                foreach (Asset a in globalAssets)
                {
                    a.Status = "Commissioned";
                    await UpdateAsset(a);
                }
            }
        }
        async void Reject_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirm Rejection", "Reject this form?", "Yes", "No");
            if (answer == true)
            {
                commission_form.RejectedBy = user_manager.ReturnName();
                commission_form.Status = "Rejected";
                await UpdateForm(commission_form);
                await Navigation.PushAsync(new ViewCommissionForms());
            }
        }
        async void Edit_Clicked(object sender, EventArgs e)
        {
            //SHOULD ONLY BE POSSIBLE IF NOT APPROVED
            await Navigation.PushAsync(new Commission(commission_form, globalAssets));
        }
        async void Exit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewCommissionForms());
        }
        async void Delete_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirm Deletion", "Delete this form?", "Yes", "No");
            if (answer == true)
            {
                //delete form and afls
                ObservableCollection<AssetFormLink> afls = await afl_manager.GetLinksByFormAsync(commission_form.Id, "Commission");
                foreach (AssetFormLink afl in afls)
                {
                    await afl_manager.DeleteLinkAsync(afl); //delete all the links
                }
                await commission_manager.DeleteFormAsync(commission_form);
                await Navigation.PushAsync(new ViewCommissionForms());
            }
        }
        private async void selectedAsset(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FormPreviewAsset((Asset)assetList.SelectedItem, 4, commission_form, globalAssets));
        }
    }
}