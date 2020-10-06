using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
//using Windows.ApplicationModel.Activation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly:XamlCompilation(XamlCompilationOptions.Compile)]
namespace K_Bikpower
{
	public class App : Application
	{
		public App ()
		{
			Device.SetFlags(new string[] { "Expander_Experimental", "RadioButton_Experimental" });
			// The root page of your application
			MainPage = (new Login());

		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

