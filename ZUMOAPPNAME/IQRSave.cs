using System;
using System.Collections.Generic;
using System.Text;

namespace K_Bikpower
{
    public interface IQRSave
    {
        void Qrcode(string value);
        QrSaved SaveQrImage();
        
 
    }
}
