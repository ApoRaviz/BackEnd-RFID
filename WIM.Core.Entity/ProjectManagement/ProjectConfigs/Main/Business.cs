using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Main
{
    public class Business
    {
        public string Type { get; set; }
        public bool IsB2B { get; set; }
        public bool IsB2C { get; set; }
        public bool IsC2C { get; set; }
    }
}
