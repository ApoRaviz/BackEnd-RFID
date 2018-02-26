using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WIM.Core.Common.ValueObject
{
    public class StatusDto 
    {
        public int StatusIDSys { get; set; }
        public string Title { get; set; }
        //public string ModuleID { get; set; }
        public virtual IEnumerable<SubModuleDto> StatusSubModule { get; set; }
    }
}
