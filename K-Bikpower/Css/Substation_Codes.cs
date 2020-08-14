using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace K_Bikpower
{
   public class Substation_Codes
    {
        [PrimaryKey]
        public string Substation_Code { get; set; } //get rid of underscore
        public string SubstationName { get; set; }
        public string SubstationTypeID { get; set; }
        public string SubstationAreaID { get; set; }
        public string SubstationAreaName { get; set; }
    }
}
