using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace K_Bikpower
{
    public class User
    {
        [PrimaryKey]

        public string UserName { get; set; } = null;
        public string Password { get; set; } = null;
        [NotNull]
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }
}
