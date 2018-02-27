using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.SpareField
{
    public class SpareField 
    {
 
        public int SpfIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string Text { get; set; }
        public string TableName { get; set; }
        public string Type { get; set; }
    }

}
