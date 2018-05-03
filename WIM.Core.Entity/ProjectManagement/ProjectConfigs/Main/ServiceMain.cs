using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Main
{
    public class ServiceMain
    {
        [MoreThanOne]
        public bool IsImport { get; set; }
        public bool IsWarehouse { get; set; }
        public bool IsPacking { get; set; }
        public bool IsTransport { get; set; }
    }
}
