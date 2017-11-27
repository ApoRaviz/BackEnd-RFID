using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Status;

namespace WIM.Core.Common.ValueObject
{
    public class StatusSubModuleDto 
    {
        public int StatusIDSys { get; set; }
        public string Title { get; set; }
        public int SubModuleIDSys { get; set; }
        public string SubModuleName { get; set; }
        public int ModuleIDSys { get; set; }
        public string ModuleName { get; set; }
    }
}
