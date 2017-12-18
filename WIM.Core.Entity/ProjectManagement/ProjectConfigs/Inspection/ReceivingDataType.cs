using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Inspection
{
    public class ReceivingDataType
    {
        public bool IsSystem { get; set; }
        public bool IsSystemByCustomer { get; set; }
        public bool IsEmail { get; set; }
        public bool IsFTP { get; set; }
        public bool IsInterface { get; set; }
        public bool IsMobileApp { get; set; }
        public bool IsRFID { get; set; }

    }
}
