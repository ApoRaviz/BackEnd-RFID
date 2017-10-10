using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Security.Entity.RoleAndPermission;

namespace WIM.Core.Security.Entity.UserManagement
{
    public class UserRole
    {
        public string UserID { get; set; }
        public string RoleID { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
