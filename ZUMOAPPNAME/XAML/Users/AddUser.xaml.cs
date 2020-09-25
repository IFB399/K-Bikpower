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
    public partial class AddUser : ContentPage
    {
        User popuserData;
        UserManager user_manager;
        public string Ids;
        public AddUser(User Userdata)
        {
            InitializeComponent();

            user_manager = UserManager.DefaultManager;
            if (Userdata != null)
            {
                AddOrUpdateButton.Text = "Update Asset";
                popuserData = Userdata;
                PopulateDetails(popuserData);
                Ids = Userdata.Id;

            }


        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Auth.SelectedIndex = 1;
        }
        private void PopulateDetails(User data)
        {
            Firstname.Text = data.FirstName;
            Lastname.Text = data.LastName;
            Email.Text = data.Email;
            Username.Text = data.Username;
        }




        async Task AddItem(User item)
        {
            await user_manager.SaveTaskAsync(item);
            //toDo.ItemsSource = await manager.GetTodoItemsAsync();
        }

        async void Add_User_Clicked(object sender, EventArgs e)
        {

            if (String.IsNullOrWhiteSpace(Firstname.Text) )
            {
                await DisplayAlert("Alert", "Please enter a first name", "OK");
                return;
            }

            if (String.IsNullOrWhiteSpace(Lastname.Text))
            {
                await DisplayAlert("Alert", "Please enter a lastname", "OK");
                return;
            }
            if (String.IsNullOrWhiteSpace(Email.Text))
            {
               await DisplayAlert("Alert", "Please enter an email address", "OK");
                return;
            }
            if (String.IsNullOrWhiteSpace(Username.Text))
            {
                await DisplayAlert("Alert", "Please enter a username", "OK");
                return;
            }

            User todo = new User
            {
                FirstName = Firstname.Text,
                LastName = Lastname.Text,
                Email = Email.Text,
                Username = Username.Text,
                Permission = Auth.SelectedItem.ToString()
        };
            var result = await user_manager.GetUser(Username.Text);
            if (result == null)
            {
                await AddItem(todo);
                await Navigation.PushAsync(new AddUser(null));
            }
            else { await DisplayAlert("Alert", "Username already exists please try another", "OK"); };
        }
    }
}