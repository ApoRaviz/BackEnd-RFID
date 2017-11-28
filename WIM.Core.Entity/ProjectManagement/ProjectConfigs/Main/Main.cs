using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Main
{
    public class Main
    {
        public Service Service { get; set; }
        public Business Business { get; set; }
        public bool IsVIPCustomer { get; set; }
        public Allocate Allocate { get; set; }
    }
}
