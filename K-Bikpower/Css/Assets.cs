using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace K_Bikpower
{
    public class Assets
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string SubstationCode { get; set; }

        public string PlantNumber { get; set; }

        public int AssetEquipmentNumber { get; set; }

        public string EquipmentStatus { get; set; }

        public string SerialNumber { get; set; }

        public string ModifierCode { get; set; }

        public int LocationEquipmentNumber { get; set; }

        public string ComponentCode { get; set; }

        public DateTime WarrantyDate { get; set; }

        public int EquipmentAge { get; set; }// needs auto timer

        public string StockCode { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public int RatedVoltage { get; set; }

        public int NominalVoltage { get; set; }

        public string ManufacturerName { get; set; }

        public string ManufacturerType { get; set; }

        public string SpecificationTitle { get; set; }

        public string SpecificationNumber { get; set; }

        public string SpecificationItemNumber { get; set; }

        public string LastInstallDate { get; set; }

        public string EquipmentClass { get; set; }

        public string EquipmentClassDescription { get; set; }
    }   
}