using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Main
{
    public class Main
    {
        
        public ServiceMain Service { get; set; }
        public Business Business { get; set; }
        public bool IsVIPCustomer { get; set; }
        public Allocate Allocate { get; set; }

        
    }

   

}
