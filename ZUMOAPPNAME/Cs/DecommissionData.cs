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
        string date; //will change data type later
        string details;
        string regionName;
        string location;
        string movedTo;
        int workOrderNumber;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "date")] 
		public string Date
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

    }
}

