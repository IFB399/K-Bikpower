using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace K_Bikpower
{
   public class Substation
    {
        string substation_code;
        string substation_name;
        string area;
        bool done;
        string id;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "substation_code")]
        public string Substation_Code {
            get {return substation_code; }
            set { substation_code = value; }
        }
        
        [JsonProperty(PropertyName = "substation_name")]
        public string Substation_Name
        {
            get { return substation_name; }
            set { substation_name = value; }
        }

        [JsonProperty(PropertyName = "area")]
        public string Area
        {
            get { return area; }
            set { area = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}
