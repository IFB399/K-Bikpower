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

        public string AssetName{ get; set; }
    }
}
