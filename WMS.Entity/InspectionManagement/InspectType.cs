using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.InspectionManagement
{
    public class InspectType
    {
        public byte InspectTypeIDSys { get; set; }
        public string InspectTypeName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }
    }
}
