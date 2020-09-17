using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace K_Bikpower.Cs
{
    public class DecommissionedAssets
    {
        string id;
        string decommissionId;
        string assetId;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty(PropertyName = "decommissionId")]
        public string DecommissionId
        {
            get { return decommissionId; }
            set { decommissionId = value; }
        }
        [JsonProperty(PropertyName = "assetId")]
        public string AssetId
        {
            get { return AssetId; }
            set { AssetId = value; }
        }
    }
}
