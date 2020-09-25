using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace K_Bikpower
{
    public class DecommissionData
    {
        string id;
        DateTime date; //will change data type later
        string details;
        string regionName;
        string location;
        string movedTo;
        int workOrderNumber;
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

        [JsonProperty(PropertyName = "details")]
        public string Details
        {
            get { return details; }
            set { details = value; }
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

        [JsonProperty(PropertyName = "movedTo")]
        public string MovedTo
        {
            get { return movedTo; }
            set { movedTo = value; }
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

    }
}

