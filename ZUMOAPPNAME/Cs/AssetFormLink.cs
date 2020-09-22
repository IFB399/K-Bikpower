using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace K_Bikpower
{
    public class AssetFormLink
    {
        string id;
        string formId;
        string assetId;
        string formType;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty(PropertyName = "formId")]
        public string FormId
        {
            get { return formId; }
            set { formId = value; }
        }
        [JsonProperty(PropertyName = "assetId")]
        public string AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }
        [JsonProperty(PropertyName = "formType")]
        public string FormType
        {
            get { return formType; }
            set { formType = value; }
        }
    }
}
