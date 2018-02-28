using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Status;

namespace WIM.Core.Common.ValueObject
{
    public class SubModuleDto 
    {
        public int StatusIDSys { get; set; }
        public int SubModuleIDSys { get; set; }
        public Nullable<int> ModuleIDSys { get; set; }
        public string SubModuleName { get; set; }
        public string ModuleName { get; set; }
        public string LabelSubModuleName { get; set; }
        //public virtual Module_MT Module_MT { get; set; }
    }
}
