
using System.Collections.Generic;
using WIM.Core.Common.Utility.Helpers;
using WIM.Core.Entity.LabelManagement.LabelConfigs;

namespace WIM.Core.Common.ValueObject
{
    public class LabelControlDto
    {

        public int LabelIDSys { get; set; }
        public int ModuleIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string Lang { get; set; }
        public string Config { get; set; }
        public List<LabelConfig> LabelConfig { get; set; }
       
      
    }
}
