using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace K_Bikpower
{
    public class User
    {
        public string UserName { get; set; } = null; //will do later not sure its needed anymore. 
        public string Password { get; set; } = null;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }
}
