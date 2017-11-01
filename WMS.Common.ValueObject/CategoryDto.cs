using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WMS.Master;


namespace WMS.Common
{
    public class CategoryDto
    {
        public int CateIDSys { get; set; }
        public string CateID { get; set; }
        public string CateName { get; set; }
        public bool IsActive { get; set; }
        public string UserUpdate { get; set; }
       
        public int ProjectIDSys { get; set; }
        public ProjectDto Project_MT { get; set; }      
    }
}
