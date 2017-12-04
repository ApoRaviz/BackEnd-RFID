using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Receiving
{
    public class SendingDataType
    {
        public bool IsEmail { get; set; }
        public bool IsFTP { get; set; }
        public bool IsInterface { get; set; }

    }
}
