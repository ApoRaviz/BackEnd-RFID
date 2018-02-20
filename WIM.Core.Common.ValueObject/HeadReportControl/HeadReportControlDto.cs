using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Attributes;

namespace WIM.Core.Common.ValueObject
{
    public class HeadReportControlDto
    {
        [HashidsAttrbute]
        public string HeadReportIDSys { get; set; }
        [HashidsAttrbute]
        public string SubModuleIDSys { get; set; }
        public string ReportName { get; set; }

        public List<Entity.LabelManagement.Label> HeadReportLabels { get; set; }   
        
    }
}
