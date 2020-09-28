using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public AddUser(User Userdata, string random)
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
            if (random != null && Userdata != null) 
            {
                popuserData = Userdata;
                Firstname.IsVisible = false;
                Lastname.IsVisible = false;
                Email.IsVisible = false;
                Username.IsVisible = false;
                Auth.IsVisible = false;
                AddOrUpdateButton.IsVisible = false;
                OldPaaassword.IsVisible = true;
                Oldpass.IsVisible = true;
                Next.IsVisible = true;
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
            //Username.Text = data.Username;
            Auth.SelectedItem = data.Permission;
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
                //Username = Username.Text,
                Password = Temp_PAssword.Text,
                Permission = Auth.SelectedItem.ToString()
            };
            var result = await user_manager.GetUser(Username.Text);
            if (result == null)
            {
                await AddItem(todo);
                await Navigation.PushAsync(new UsersPage());
            }
            else { await DisplayAlert("Alert", "Username already exists please try another", "OK"); };
        }

        async void Next_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Username.Text))
            {
                await DisplayAlert("Alert", "Inncorrect Password", "OK");
                return;
            }
            if (CheckPassword(OldPaaassword.Text, popuserData.Password, popuserData.Salt) == true)
            {
                OldPaaassword.IsVisible = false;
                Oldpass.IsVisible = false;
                Next.IsVisible = false;
                Newpass.IsVisible = true;
                NewPaaassword.IsVisible = true;
                Changepass.IsVisible = true;
            }
            else
            { }
            
        }

        public bool CheckPassword(string password, string correctHash, string correctsalt)
        {
            string hashpass = HashPassword(password, correctsalt);

            return (correctHash == hashpass);
        }

        string HashPassword(string pass, string salt)
        {
            SHA256Managed hash = new SHA256Managed();
            byte[] utf8 = UTF8Encoding.UTF8.GetBytes(pass + salt);
            StringBuilder s = new StringBuilder(hash.ComputeHash(utf8).Length * 2);
            foreach (byte b in hash.ComputeHash(utf8))
                s.Append(b.ToString("x2"));
            return s.ToString();
        }

        async void Changepass_Clicked(object sender, EventArgs e)
        {
            User todo = new User
            {
                FirstName = Firstname.Text,
                LastName = Lastname.Text,
                Email = Email.Text,
                //Username = Username.Text,
                Password = NewPaaassword.Text,
                Permission = Auth.SelectedItem.ToString()
            };
                await AddItem(todo);
                await Navigation.PushAsync(new MainPage());
        }
    }
}