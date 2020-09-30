using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace K_Bikpower
{
	public class Asset
	{
		string id;
		string substationCode;
		string plantNumber;
		int assetEQNO;
        string eqStatus;
        string serialNumber;
        string modifierCode;
        int locationEQNO;
        string componentCode;
        DateTime? date; //warranty date
        int equipmentAge;
        string stockCode;
        string poNO;
        int ratedVoltage;
        int nominalVoltage;
        string manufacturerName;
        string manufacturerType; //was left out
        string specificationTitle;
        string specificationNO;
        string specificationItemNO;
        DateTime lastInstallDate;
        string equipmentClass;
        string equipmentClassDescription;
        string status;
        string modifiedBy;
        string addedBy;

        [JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value;}
		}

		[JsonProperty(PropertyName = "substation_code")] ///my version
		public string SubstationCode
		{
			get { return substationCode; }
			set { substationCode = value;}
		}

		[JsonProperty(PropertyName = "plant_number")]
		public string PlantNumber
		{
			get { return plantNumber; }
			set { plantNumber = value;}
		}

		[JsonProperty(PropertyName = "asset_eq_no")]
		public int AssetEQNO
		{
			get { return assetEQNO; }
			set { assetEQNO = value; }
		}
        [JsonProperty(PropertyName = "eq_status")]
        public string EQStatus
        {
            get { return eqStatus; }
            set { eqStatus = value; }
        }


        [JsonProperty(PropertyName = "serial_number")]
        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }


        [JsonProperty(PropertyName = "modifier_code")]
        public string ModifierCode
        {
            get { return modifierCode; }
            set { modifierCode = value; }
        }


        [JsonProperty(PropertyName = "location_equipment_number")]
        public int LocationEquipmentNumber
        {
            get { return locationEQNO; }
            set { locationEQNO = value; }
        }


        [JsonProperty(PropertyName = "component_code")]
        public string ComponentCode
        {
            get { return componentCode; }
            set { componentCode = value; }
        }

        
        [JsonProperty(PropertyName = "date")]
        public DateTime? Date
        {
            get { return date; }
            set { date = value; }
        }
        


        [JsonProperty(PropertyName = "equipment_age")]
        public int EquipmentAge
        {
            get { return equipmentAge; }
            set { equipmentAge = value; }
        }


        [JsonProperty(PropertyName = "stock_code")]
        public string StockCode
        {
            get { return stockCode; }
            set { stockCode = value; }
        }

        [JsonProperty(PropertyName = "po_no")]
        public string PurchaseOrderNO
        {
            get { return poNO; }
            set { poNO = value; }
        }

        [JsonProperty(PropertyName = "rated_volts")]
        public int RatedVoltage
        {
            get { return ratedVoltage; }
            set { ratedVoltage = value; }
        }

        [JsonProperty(PropertyName = "nominal_volts")]
        public int NominalVoltage
        {
            get { return nominalVoltage; }
            set { nominalVoltage = value; }
        }

        [JsonProperty(PropertyName = "manufacturer_name")]
        public string ManufacturerName
        {
            get { return manufacturerName; }
            set { manufacturerName = value; }
        }

        [JsonProperty(PropertyName = "manufacturer_type")]
        public string ManufacturerType
        {
            get { return manufacturerType; }
            set { manufacturerType = value; }
        }

        [JsonProperty(PropertyName = "specification_title")]
        public string SpecificationTitle
        {
            get { return specificationTitle; }
            set { specificationTitle = value; }
        }

        [JsonProperty(PropertyName = "specification_no")]
        public string SpecificationNO
        {
            get { return specificationNO; }
            set { specificationNO = value; }
        }

        [JsonProperty(PropertyName = "specification_item_no")]
        public string SpecificationItemNO
        {
            get { return specificationItemNO; }
            set { specificationItemNO = value; }
        }

        [JsonProperty(PropertyName = "last_install_date")]
        public DateTime LastInstallDate
        {
            get { return lastInstallDate; }
            set { lastInstallDate = value; }
        }

        [JsonProperty(PropertyName = "equipment_class")]
        public string EquipmentClass
        {
            get { return equipmentClass; }
            set { equipmentClass = value; }
        }

        [JsonProperty(PropertyName = "equimpent_class_decription")]
        public string EquipmentClassDescription
        {
            get { return equipmentClassDescription; }
            set { equipmentClassDescription = value; }
        }

        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        [JsonProperty(PropertyName = "added_by")]
        public string AddedBy
        {
            get { return addedBy; }
            set { addedBy = value; }
        }

        [JsonProperty(PropertyName = "modified_by")]
        public string ModifiedBy
        {
            get { return modifiedBy; }
            set { modifiedBy = value; }
        }

        [Version]
        public string Version { get; set; }
	}
}

