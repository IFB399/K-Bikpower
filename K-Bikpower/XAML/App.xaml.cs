using System;
using Xamarin.Forms;
using System.IO;
using Xamarin.Forms.Xaml;


namespace K_Bikpower
{
    public partial class App : Application
    {

        
        public App()
        {

            Device.SetFlags(new string[] { "Expander_Experimental", "RadioButton_Experimental" });


            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
