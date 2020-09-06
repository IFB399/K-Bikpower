using System;

using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace K_Bikpower
{
   public class Substations
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

        /*[JsonProperty(PropertyName = "substation_code")]
        public string SubstationCode {
            get {return substation_code; }
            set { substation_code = value; }
        }
        
        [JsonProperty(PropertyName = "substation_name")]
        public string SubstationName
        {
            get { return substation_name; }
            set { substation_name = value; }
        }

        [JsonProperty(PropertyName = "area")]
        public string Aarea
        {
            get { return area; }
            set { area = value; }
        }

        [JsonProperty(PropertyName = "done")]
        public bool Done
        {
            get { return done; }
            set { done = value; }
        }
        */
        [Version]
        public string Version { get; set; }
    }
}
