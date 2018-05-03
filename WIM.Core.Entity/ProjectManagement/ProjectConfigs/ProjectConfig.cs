using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs
{
    public class ProjectConfig
    {
        public Main.Main Main { get; set; }
        public WMS.WMS WMS { get; set; }
        public TMS.TMS TMS { get; set; }

    }
}
