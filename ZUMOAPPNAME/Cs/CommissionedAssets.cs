using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace K_Bikpower.Cs
{
    public class CommissionedAssets
    {
        string id;
        string commissionId;
        string assetId;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty(PropertyName = "commissionId")]
        public string CommissionId
        {
            get { return commissionId; }
            set { commissionId = value; }
        }
        [JsonProperty(PropertyName = "assetId")]
        public string AssetId
        {
            get { return AssetId; }
            set { AssetId = value; }
        }
    }
}
