using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace K_Bikpower
{
    public class Assets
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Substation_Code { get; set; }

        public string Plant_Number { get; set; }

        public int Asset_EQ_NO { get; set; }

        public string EQ_Status { get; set; }

        public string Serial_Number { get; set; }

        public string Modifier_code { get; set; }

        public int Location_Equipment_Number { get; set; }

        public string Component_Code { get; set; }

        public string WarrantyDate { get; set; }

        public int Equipement_age { get; set; }// needs auto timer

        public string Stock_Code { get; set; }

        public string PO_NO { get; set; }

        public int Rated_Voltage { get; set; }

        public int Nominal_Voltage { get; set; }

        public string Manufacture_Name { get; set; }

        public string Specifiaction_title { get; set; }

        public string Specifiaction_NO { get; set; }

        public string Specifiaction_item_NO { get; set; }

        public string last_install_date { get; set; }

        public string Equipment_class { get; set; }

        public string Equipment_class_description { get; set; }

    }
}