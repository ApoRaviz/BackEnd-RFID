using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Repository;

namespace WMS.Common
{
    public class ApiMenuMappingDto: BaseEntityDto
    {
        public string ApiIDSys { get; set; }
        public int MenuIDSys { get; set; }
        public string ApiName { get; set; }
        public byte GET { get; set; }
        public byte POST { get; set; }
        public byte PUT { get; set; }
        public byte DEL { get; set; }
        public string Type { get; set; }
    }
}
