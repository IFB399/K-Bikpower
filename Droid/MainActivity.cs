﻿using System;

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
using Android;

using Xamarin.Forms.Internals;

namespace K_Bikpower.Droid
{

	[Activity (Label = "K_Bikpower.Droid",
		Icon = "@drawable/homeicon",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		Theme = "@style/MainTheme")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		// Define an authenticated user.
		
        
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Registrar.Registered.Register(typeof(Xamarin.Forms.CheckBox), typeof(Xamarin.Forms.Platform.Android.CheckBoxRenderer));
			Registrar.Registered.Register(typeof(Xamarin.Forms.RadioButton), typeof(Xamarin.Forms.Platform.Android.RadioButtonRenderer));
			// Initialize Azure Mobile Apps
			CurrentPlatform.Init();
			Xamarin.Essentials.Platform.Init(Application);
			ZXing.Net.Mobile.Forms.Android.Platform.Init();

			// Initialize Xamarin Forms
			Forms.Init (this, bundle);

			Xamarin.Essentials.Platform.Init(this, bundle); // add this line to your code, it may also be called: bundle

			// Load the main application
			CheckAppPermissions();
            LoadApplication (new App ());
		}
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

		}
		private void CheckAppPermissions()
		{
			if ((int)Build.VERSION.SdkInt < 23)
			{
				return;
			}
			else
			{
				if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
					&& PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
				{
					var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
					RequestPermissions(permissions, 1);
				}
			}
		}
	}
}

