﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Windows.Media.Import;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageFormAssets : ContentPage
    {
        //DecommissionData dform;
        Object form;
        ObservableCollection<Asset> globalAssets = new ObservableCollection<Asset>();
        bool prevPage;
        public ManageFormAssets(Object o, ObservableCollection<Asset> assets, bool fromApprovalPage = false)
        {
            InitializeComponent();
            //dform = d;
            form = o;
            globalAssets = assets;
            assetList.ItemsSource = assets;
            prevPage = fromApprovalPage;
            if (prevPage == true) //not allowing approver to update because cbb updating asset form links :/
            {
                ScanButton.IsEnabled = false;
                removeAsset.IsEnabled = false;
                AddButton.IsEnabled = false;

            }
        }
        private void removeAsset_Clicked(object sender, EventArgs e)
        {
            globalAssets.Remove((Asset)assetList.SelectedItem);
            removeAsset.IsEnabled = false;
        }
        private void selectedAsset(object sender, EventArgs e)
        {
            removeAsset.IsEnabled = true;
        }
        private void Scan_Asset_Clicked(object sender, EventArgs e)
        {
            
            Navigation.PushAsync(new ScanQR(form, globalAssets, prevPage)); //go to scan page

        }
        private async void Add_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new AssetList(form, globalAssets, prevPage)); //go to table page

        }
        private void Done_Clicked(object sender, EventArgs e)
        {
            if (prevPage == false)
            {
                if (typeof(DecommissionData).IsInstanceOfType(form))
                {
                    DecommissionData d = (DecommissionData)form;
                    Navigation.PushAsync(new Decommission(d, globalAssets)); //return to correct page
                }
                else //return to commission page
                {
                    CommissionData c = (CommissionData)form;
                    Navigation.PushAsync(new Commission(c, globalAssets)); //return to correct page
                }
            }
            else
            {
                if (typeof(DecommissionData).IsInstanceOfType(form))
                {
                    DecommissionData d = (DecommissionData)form;
                    Navigation.PushAsync(new ApproveDecommission(d, globalAssets)); //return to correct page
                }
                else //go to approve commission page TO BE COMPLETED
                {
                    CommissionData c = (CommissionData)form;
                    //go to page
                }
                    
            }
        }
    }
}