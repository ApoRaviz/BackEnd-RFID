using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.RoleAndPermission;

namespace WIM.Core.Common.ValueObject
{
    public class RolePermissionDto
    {
        public string RoleID { get; set; }
        public string PermissionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSysAdmin { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
