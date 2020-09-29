using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace K_Bikpower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class main : MasterDetailPage
    {
        public main()
        {
            InitializeComponent();
           
            masterPage.listView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;

            if (item != null)
            {
                if(item.Title == "Assets")
                {
                    Detail = new NavigationPage(new AssetList());
                    masterPage.listView.SelectedItem = null;
                    IsPresented = false;
                    return;
                }
                if (item.Title == "Qr Code Scanner")
                {
                    Detail = new NavigationPage(new ScanQR());
                    masterPage.listView.SelectedItem = null;
                    IsPresented = false;
                    return;
                }
                else
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                    masterPage.listView.SelectedItem = null;
                    IsPresented = false;
                }
            }
        }
    }

}
