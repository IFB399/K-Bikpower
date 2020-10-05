using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Security.Cryptography;
using System.Collections.ObjectModel;

namespace K_Bikpower
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {

        UserManager manager;
        public Login()
        {
            InitializeComponent();
            manager = UserManager.DefaultManager;
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
        public bool CheckPassword(string password, string correctHash, string correctsalt)
        {
            string hashpass = HashPassword(password, correctsalt);

            return (correctHash == hashpass);
        }

        async void loginButton_Clicked(object sender, EventArgs e)
        {
            string email = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            ObservableCollection<User> userList = await manager.GetUser(email);
            User u = userList.FirstOrDefault();
            if (u == null)
            {
                await DisplayAlert("Incorrect", "User not identified", "Close");
            }
            else
            {
                string salt = u.Salt;

                if (CheckPassword(password, u.Password, salt) == true)
                {
                    manager.SetUsername(email);
                    manager.Setname(u.FirstName, u.LastName);
                    await manager.GetUserAuth(email);
                    App.Current.MainPage = new main();
                }
                else
                {
                    //display an error
                    await DisplayAlert("Incorrect", "Incorrect username or password", "Close");
                }
            }
        }

    }
}