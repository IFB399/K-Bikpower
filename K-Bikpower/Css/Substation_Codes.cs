using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace K_Bikpower
{
   public class Substation_Codes
    {
        string substation_code;
        string substation_name;
        string area;
        public string Substation_Code {
            get {return substation_code; }
            set { substation_code = value; }
        }

        public string Substation_Name
        {
            get { return substation_name; }
            set { substation_name = value; }
        }
        public string Area
        {
            get { return area; }
            set { area = value; }
        }
    }
}
