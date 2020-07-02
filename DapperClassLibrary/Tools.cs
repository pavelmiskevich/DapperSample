using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperClassLibrary
{
    public static class Tools
    {
        public static string GetConnectionString(string name = "cs")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
