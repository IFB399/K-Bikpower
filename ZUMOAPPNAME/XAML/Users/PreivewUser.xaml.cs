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
            string auth = user_manager.Authentication();
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
            if (auth == "Administrator") 
            {
                DeleteButton.IsVisible = true;
            }
        }
        private async void GetUserData ()
        {
            string username = user_manager.ReturnUser();
            ObservableCollection<User> userList = await user_manager.GetUser(username);
            User u = userList.FirstOrDefault();
            PopulateDetails(u);
            popuserData = u;
            randomstring = "Warp Tach is Rad";
        }
        private void PopulateDetails(User data)
        {
            Firstname.Text = data.FirstName;
            Lastname.Text = data.LastName;
            Email.Text = data.Email;
            Auth.Text = data.Role;
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddUser(popuserData, randomstring));
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            await user_manager.DeleteUserAsync(popuserData);
            await Navigation.PushAsync(new UsersPage());
        }
    }
}