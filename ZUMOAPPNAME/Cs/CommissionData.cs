using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace K_Bikpower
{
    public class CommissionData
    {
        string id;
        DateTime dateCommissioned;
        string newInstallation;
        string replacement;
        string regionName;
        string location;
        string movedFrom;
        int workOrderNumber;
        string submittedBy;
        string status;
        string approvedBy;
        string rejectedBy;


        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "dateCommissioned")]
        public DateTime DateCommissioned
        {
            get { return dateCommissioned; }
            set { dateCommissioned = value; }
        }

        [JsonProperty(PropertyName = "newInstallation")]
        public string NewInstallation
        {
            get { return newInstallation; }
            set { newInstallation = value; }
        }

        [JsonProperty(PropertyName = "replacement")]
        public string Replacement
        {
            get { return replacement; }
            set { replacement = value; }
        }

        [JsonProperty(PropertyName = "regionName")]
        public string RegionName
        {
            get { return regionName; }
            set { regionName = value; }
        }

        [JsonProperty(PropertyName = "location")]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        [JsonProperty(PropertyName = "movedFrom")]
        public string MovedFrom
        {
            get { return movedFrom; }
            set { movedFrom = value; }
        }

        [JsonProperty(PropertyName = "workOrderNumber")]
        public int WorkOrderNumber
        {
            get { return workOrderNumber; }
            set { workOrderNumber = value; }
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

        [JsonProperty(PropertyName = "approvedBy")]
        public string ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        [JsonProperty(PropertyName = "rejectedBy")]
        public string RejectedBy
        {
            get { return rejectedBy; }
            set { rejectedBy = value; }
        }
    }
}
