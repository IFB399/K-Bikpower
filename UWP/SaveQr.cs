using K_Bikpower;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP;
using Xamarin.Forms.Internals;
using ZXing.Net.Mobile;
using ZXing.Net;
using static System.Net.Mime.MediaTypeNames;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;

using Windows.Storage;
using Windows.Storage.Pickers;

[assembly: Xamarin.Forms.Dependency(typeof(SaveQr))]
namespace UWP
{
    class SaveQr: IQRSave
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
            try 
            { string result =  SaveQRAsImageAsync(text).ToString();
                if (result == "success") { return saved; } else { return notsaved; }
            }
            catch (Exception)
            {
                return notsaved;
            }
        }

        private async Task<string> WriteableBitmapToStorageFile(WriteableBitmap WB)
        {
           
            Guid BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
            //var file = await Windows.Storage. CreateFileAsync(FileName, CreationCollisionOption.GenerateUniqueName);
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Png-Image", new List<string>() { ".png"});
            savePicker.FileTypeChoices.Add("Jpeg-Image", new List<string>() { ".jpeg" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "MY_QR" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff");

            StorageFile file = await savePicker.PickSaveFileAsync();
            try
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                    Stream pixelStream = WB.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                                        (uint)WB.PixelWidth,
                                        (uint)WB.PixelHeight,
                                        96.0,
                                        96.0,
                                        pixels);
                    await encoder.FlushAsync();
                }
                return "success";
            }
            catch (Exception)
            {
                return "fail"; 
            }
        }

        public async Task<string> SaveQRAsImageAsync(string text)
        {

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
                    },
                    Renderer = new ZXing.Mobile.WriteableBitmapRenderer() { Foreground = Windows.UI.Colors.Black }
                };
                var writeableBitmap = barcodeWriter.Write(text);

                var something = await WriteableBitmapToStorageFile(writeableBitmap);

                 if (something == "fail") { return "fail"; }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
                return "fail";
            }
            return "success";
        }



    }
}
