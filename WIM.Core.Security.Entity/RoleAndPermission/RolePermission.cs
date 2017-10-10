using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Security.Entity.RoleAndPermission
{
    public class RolePermission
    {
        public string RoleID { get; set; }
        public string PermissionID { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
