using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace K_Bikpower
{
    public class User
    {
        [PrimaryKey]
        public string UserName { get; set; }
        public string Password { get; set; } 

        public DateTime LastLogin { get; set; }
    }
}
