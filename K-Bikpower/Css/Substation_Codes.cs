using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace K_Bikpower
{
   public class Substation_Codes
    {
        [PrimaryKey]
        public string Substation_Code { get; set; }
        public string Substation_Name { get; set; }

        public string Area { get; set; }
    }
}
