using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using UIKit;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace K_Bikpower.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
       
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return AssetManager.DefaultManager.CurrentClient.ResumeWithURL(url);
        }
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// Initialize Azure Mobile Apps
			CurrentPlatform.Init();

			// Initialize Xamarin Forms
			Forms.Init();


            LoadApplication(new App ());

			return base.FinishedLaunching(app, options);
		}
	}
}

