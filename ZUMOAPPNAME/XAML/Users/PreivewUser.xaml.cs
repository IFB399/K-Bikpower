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
    public partial class PreviewUser : ContentPage
    {
        User popuserData;
        UserManager user_manager;
        string randomstring;
        public string Ids;
        public  PreviewUser(User Userdata)
        {
            InitializeComponent();

            user_manager = UserManager.DefaultManager;
            if (Userdata != null)
            {

                popuserData = Userdata;
                PopulateDetails(popuserData);
                Ids = Userdata.Id;

            }
            if (Userdata == null)
            {
                GetUserData();
            }
            }
        private async void GetUserData ()
        {
            string username = user_manager.ReturnUser();
            ObservableCollection<User> userList = await user_manager.GetUser(username);
            User u = userList.FirstOrDefault();
            PopulateDetails(u);
            randomstring = "Warp Tach is Rad";
        }
        private void PopulateDetails(User data)
        {
            Firstname.Text = data.FirstName;
            Lastname.Text = data.LastName;
            Email.Text = data.Email;
            Username.Text = data.Username;
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddUser(popuserData, randomstring));
        }
    }
}