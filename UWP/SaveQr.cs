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
using ZXing;
using ZXing.Common;
using static System.Net.Mime.MediaTypeNames;
[assembly: Xamarin.Forms.Dependency(typeof(SaveQr))]
namespace UWP
{
    class SaveQr: IQRSave
    {
        private QrSaved notsaved;
        private QrSaved saved;

        public void Qrcode(string value)
        {
            throw new NotImplementedException();
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

                    return SaveQRComplete.Task;
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                    return null;
                }
            }
            string text2 = "im not allowed to write swear words cause i will forget to remove them :(";
            try { SaveQRAsImage(text2); }
            catch (Exception ex)
            {
                return notsaved;
            }
            return saved;
        } 

       

    }
}
