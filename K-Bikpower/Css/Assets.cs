using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace K_Bikpower
{
    public class Assets
    {
        int id;
        string substation_code;
        string plant_Number;
        int asset_EQ_NO;
        string eQ_status;
        string serial_number;
        string modifier_code;
        int location_equipment_number;
        string component_code;
        string warrantydate;
        int equipement_age;
        string stock_code;
        string pO_nO;
        int rated_Voltage;
        int nominal_Voltage;
        string manufacture_Name;
        string specifiaction_title;
        string specifiaction_NO;
        string specifiaction_item_NO;
        string Last_install_date;
        string equipment_class;
        string equipment_class_description;
        bool done;


        [JsonProperty(PropertyName = "id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Substation_Code
        {
            get { return substation_code; }
            set { substation_code = value; }
        }

        public string Plant_Number
        {
            get { return plant_Number; }
            set { plant_Number = value; }
        }

        public int Asset_EQ_NO
        {
            get { return asset_EQ_NO; }
            set { asset_EQ_NO = value; }
        }

        public string EQ_Status
        {
            get { return eQ_status; }
            set { eQ_status = value; }
        }

        public string Serial_Number
        {
            get { return serial_number; }
            set { serial_number = value; }
        }


        public string Modifier_code
        {
            get { return modifier_code ; }
            set { modifier_code = value; }
        }

        public int Location_Equipment_Number
        {
            get { return location_equipment_number; }
            set { location_equipment_number = value; }
        }

        public string Component_Code
        {
            get { return component_code; }
            set { component_code = value; }
        }

        public string WarrantyDate
        {
            get { return warrantydate; }
            set { warrantydate = value; }
        }

        public int Equipement_age
        {
            get { return equipement_age; }
            set { equipement_age = value; }
        }

        public string Stock_Code
        {
            get { return stock_code; }
            set { stock_code = value; }
        }
        public string PO_NO
        {
            get { return pO_nO; }
            set { pO_nO = value; }
        }
        public int Rated_Voltage
        {
            get { return rated_Voltage; }
            set { rated_Voltage = value; }
        }
        public int Nominal_Voltage
        {
            get { return nominal_Voltage; }
            set { nominal_Voltage = value; }
        }
        public string Manufacture_Name
        {
            get { return manufacture_Name; }
            set { manufacture_Name = value; }
        }
        public string Specifiaction_title
        {
            get { return specifiaction_title; }
            set { specifiaction_title = value; }
        }
        public string Specifiaction_NO
        {
            get { return specifiaction_NO; }
            set { specifiaction_NO = value; }
        }
        public string Specifiaction_item_NO
        {
            get { return specifiaction_item_NO; }
            set { specifiaction_item_NO = value; }
        }
        public string last_install_date
        {
            get { return Last_install_date; }
            set { Last_install_date = value; }
        }
        public string Equipment_class
        {
            get { return equipment_class; }
            set { equipment_class = value; }
        }
        public string Equipment_class_description
        {
            get { return equipment_class_description; }
            set { equipment_class_description = value; }
        }
        public bool Done
        {
            get { return done; }
            set { done = value; }
        }
    }   
}