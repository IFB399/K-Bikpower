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
    public partial class PreviewUser : ContentPage
    {
        User popuserData;
        UserManager user_manager;
        public string Ids;
        public PreviewUser(User Userdata)
        {
            InitializeComponent();

            user_manager = UserManager.DefaultManager;
            if (Userdata != null)
            {

                popuserData = Userdata;
                PopulateDetails(popuserData);
                Ids = Userdata.Id;

            }


        }

        private void PopulateDetails(User data)
        {
            Firstname.Text = data.FirstName;
            Lastname.Text = data.LastName;
            Email.Text = data.Email;
            //Username.Text = data.Username;
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddUser(popuserData));
        }
    }
}