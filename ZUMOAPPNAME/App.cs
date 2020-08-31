using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly:XamlCompilation(XamlCompilationOptions.Compile)]
namespace K_Bikpower
{

	public interface IAuthenticate
	{
		Task<bool> Authenticate();
	}
	public class App : Application
	{

		public static IAuthenticate Authenticator { get; private set; }

		public static void Init(IAuthenticate authenticator)
		{
			Authenticator = authenticator;
		}

		
		public App ()
		{
			// The root page of your application
			MainPage = new NavigationPage(new Login());
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

