using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

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
                AddOrUpdateButton.Text = "Update User";
                popuserData = Userdata;
                PopulateDetails(popuserData);
                Ids = Userdata.Id;

            }
            if (random != null) 
            {
                
                popuserData = Userdata;
                Firstname.IsVisible = false;
                Lastname.IsVisible = false;
                Email.IsVisible = false;
                Auth.IsVisible = false;
                AddOrUpdateButton.IsVisible = false;
                Firstnamelabel.IsVisible = false;
                LastnameLabel.IsVisible = false;
                Emaillabel.IsVisible = false;
                Temp_PAsswordLabel.IsVisible = false;
                Temp_PAssword.IsVisible = false;
                Authlabel.IsVisible = false;


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
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(Temp_PAssword.Text, salt);
            User todo = new User
            {
                Id = Ids,
                FirstName = Firstname.Text,
                LastName = Lastname.Text,
                Email = Email.Text,
                //Username = Username.Text,
                Salt = salt,
                Password = hashedPassword,
                Permission = Auth.SelectedItem.ToString()
            };
            ObservableCollection<User> u = await user_manager.GetUser(Email.Text);
            User user = u.FirstOrDefault();
            if (AddOrUpdateButton.Text == "Update User")
            {
                await AddItem(todo);
                await Navigation.PushAsync(new UsersPage());
            }
            
            if (user == null)
            {
                await AddItem(todo);
                await Navigation.PushAsync(new UsersPage());
            }

            else { await DisplayAlert("Alert", "Email already exists please try another", "OK"); };
        }

        async void Next_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(OldPaaassword.Text))
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
        string GenerateSalt()
        {
            byte[] buf = new byte[32];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
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
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(NewPaaassword.Text, salt);
            User todo = new User
            {
                Id = Ids,
                FirstName = Firstname.Text,
                LastName = Lastname.Text,
                Email = Email.Text,
                //Username = Username.Text,
                Password = hashedPassword,
                Salt = salt,
                Permission = Auth.SelectedItem.ToString()
            };
                await AddItem(todo);
                await Navigation.PushAsync(new MainPage());
        }
    }
}