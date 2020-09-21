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
    /*public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }*/
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {

        /*public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }*/

        //bool authenticated = false;
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
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            ObservableCollection<User> userList = await manager.GetUser(username);
            User u = userList.FirstOrDefault();
            //User u = await manager.GetUser(username);
            if (u == null)
            {
                await DisplayAlert("Incorrect", "User not identified", "Close");
            }
            else
            {
                string salt = u.Salt;

                if (CheckPassword(password, u.Password, salt) == true)
                {
                    manager.SetUsername(username);
                    await manager.GetUserAuth(username);
                    App.Current.MainPage = new main();
                }
                else
                {
                    //display an error
                    await DisplayAlert("Incorrect", "Incorrect username or password", "Close");
                }
            }

            /*
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (authenticated == true)
                await Navigation.PushAsync(new MainPage());
            */

        }

    }
}