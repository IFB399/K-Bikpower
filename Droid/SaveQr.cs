using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using K_Bikpower.Droid;
using Xamarin.Forms.Internals;

[assembly: Xamarin.Forms.Dependency(typeof(SaveQr))]
namespace K_Bikpower.Droid
{
    class SaveQr: IQRSave
    {
        string text;
        private QrSaved notsaved;
        private QrSaved saved;

        public void Qrcode(string value)
        {
            text = value;
        }

        public QrSaved SaveQrImage()
        {
            TaskCompletionSource<string> SaveQRComplete = null;

            Task SaveQRAsImage(string text)
            {
                SaveQRComplete = new TaskCompletionSource<string>();
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
                    var stream = new MemoryStream();
                    bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);  // this is the diff between iOS and Android
                    stream.Position = 0;

                    byte[] imageData = stream.ToArray();

                    var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                    var pictures = dir.AbsolutePath;
                    //adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name
                    string name = "MY_QR" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                    string filePath = System.IO.Path.Combine(pictures, name);

                    System.IO.File.WriteAllBytes(filePath, imageData);
                    //mediascan adds the saved image into the gallery
                    var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(filePath)));

                    // Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
                    SaveQRComplete.SetResult(filePath);
                    return SaveQRComplete.Task;
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                    return null;
                }
            }
            string text2 = "im not allowed to write swear words cause i will forget to remove them :(";
            try { SaveQRAsImage(text); }
            catch (Exception ex)
            {
                return notsaved;
            }
            return saved;
        }
        



    }
}