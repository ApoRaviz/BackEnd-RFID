using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Main
{
    public class Service
    {
        public bool IsImport { get; set; }
        public bool IsWarehouse { get; set; }
        public bool IsPacking { get; set; }
        public bool IsTransport { get; set; }
    }
}
