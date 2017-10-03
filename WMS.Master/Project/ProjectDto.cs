using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Master.Customer;
using WMS.Repository;

namespace WMS.Master
{
    public class ProjectDto : BaseEntityDto
    {
        public int ProjectIDSys { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }

        public int CusIDSys { get; set; }
        public virtual CustomerDto Customer_MT { get; set; }

    }
}
