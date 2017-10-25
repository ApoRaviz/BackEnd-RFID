using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common
{
    public class ApiMenuMappingDto
    {
        public string ApiIDSys { get; set; }
        public int MenuIDSys { get; set; }
        public string ApiName { get; set; }
        public bool GET { get; set; }
        public bool POST { get; set; }
        public bool PUT { get; set; }
        public bool DEL { get; set; }
        public string Type { get; set; }
    }
}