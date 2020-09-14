using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using K_Bikpower;
using UIKit;
using Xamarin.Forms.Internals;
using ZUMOAPPNAME.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(SaveQr))]

namespace ZUMOAPPNAME.iOS
{
    class SaveQr : IQRSave
    {
        private QrSaved notsaved;
        private QrSaved saved;
        string text;
        public void Qrcode(string value)
        {
            text = value;
        }

        public QrSaved SaveQrImage()
        {
            Task SaveMyQR(string text)
            {
                TaskCompletionSource<string> SaveQRComplete = new TaskCompletionSource<string>();
                try
                {
                    var barcodeWriter = new ZXing.Mobile.BarcodeWriter
                    {
                        Format = ZXing.BarcodeFormat.QR_CODE,
                        Options = new ZXing.Common.EncodingOptions
                        {
                            Width = 1000,
                            Height = 1000,
                            Margin = 10
                        }
                    };

                    barcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
                    var bitmap = barcodeWriter.Write(text);
                    var stream = bitmap.AsPNG().AsStream();

                    byte[] imageData = bitmap.AsPNG().ToArray();


                    var chartImage = new UIImage(NSData.FromArray(imageData));
                    chartImage.SaveToPhotosAlbum((image, error) =>
                    {

                    //you can retrieve the saved UI Image as well if needed using
                    //var i = image as UIImage;
                    if (error != null)
                        {
                            Console.WriteLine(error.ToString());
                        }

                    });
                    SaveQRComplete.SetResult("true");

                }
                catch (Exception ex)
                {
                    SaveQRComplete.SetResult("false");
                }
                return SaveQRComplete.Task;
            }
            try { SaveMyQR(text); }
            catch (Exception ex)
            {
                return notsaved;
            }
            return saved;
        }
    }
}