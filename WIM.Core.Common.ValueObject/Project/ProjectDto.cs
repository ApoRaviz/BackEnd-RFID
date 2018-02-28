using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WIM.Core.Common.ValueObject
{
    public class ProjectDto 
    {
        public int ProjectIDSys { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }

        public int CusIDSys { get; set; }
        public virtual CustomerDto Customer_MT { get; set; }

    }
}
