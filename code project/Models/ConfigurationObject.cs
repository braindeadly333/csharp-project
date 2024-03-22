using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code_project.Models
{
    internal class ConfigurationObject
    {
        public static string Uri => ConfigurationManager.AppSettings["uri"];
    }
}
