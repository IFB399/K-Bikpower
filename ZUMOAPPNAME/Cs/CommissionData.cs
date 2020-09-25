using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace K_Bikpower
{
    public class CommissionData
    {
        string id;
        DateTime date;
        string substationName;
        string substationCode;
        string operationalId;
        string operationalDescription;
        string voltage;
        string insulation;
        string equipmentClass;
        string rating;
        string submittedBy;
        string status;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        [JsonProperty(PropertyName = "substationName")]
        public string SubstationName
        {
            get { return substationName; }
            set { substationName = value; }
        }

        [JsonProperty(PropertyName = "substationCode")]
        public string SubstationCode
        {
            get { return substationCode; }
            set { substationCode = value; }
        }

        [JsonProperty(PropertyName = "operationalId")]
        public string OperationalId
        {
            get { return operationalId; }
            set { operationalId = value; }
        }

        [JsonProperty(PropertyName = "operationalDescription")]
        public string OperationalDescription
        {
            get { return operationalDescription; }
            set { operationalDescription = value; }
        }

        [JsonProperty(PropertyName = "voltage")]
        public string Voltage
        {
            get { return voltage; }
            set { voltage = value; }
        }

        [JsonProperty(PropertyName = "insulation")]
        public string Insulation
        {
            get { return insulation; }
            set { insulation = value; }
        }

        [JsonProperty(PropertyName = "equipmentClass")]
        public string EquipmentClass
        {
            get { return equipmentClass; }
            set { equipmentClass = value; }
        }

        [JsonProperty(PropertyName = "rating")]
        public string Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        [JsonProperty(PropertyName = "submittedBy")]
        public string SubmittedBy
        {
            get { return submittedBy; }
            set { submittedBy = value; }
        }

        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

    }
}
