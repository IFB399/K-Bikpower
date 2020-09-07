using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.WindowsAzure.MobileServices;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Threading.Tasks;

namespace K_Bikpower.Droid
{
	[Activity (Label = "K_Bikpower.Droid",
		Icon = "@drawable/icon",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
        // Define an authenticated user.

        
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Initialize Azure Mobile Apps
			CurrentPlatform.Init();
			Xamarin.Essentials.Platform.Init(Application);
			ZXing.Net.Mobile.Forms.Android.Platform.Init();
			// Initialize Xamarin Forms
			Forms.Init (this, bundle);
            // Load the main application
            LoadApplication (new App ());
		}
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}

