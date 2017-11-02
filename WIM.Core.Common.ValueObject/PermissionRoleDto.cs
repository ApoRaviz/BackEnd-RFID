using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.ValueObject
{
    public class PermissionRoleDto
    {
        public string PermissionID { get; set; }
        public string PermissionName { get; set; }
        public List<MenuProjectMappingDto> MenuProjectMapping { get; set; }
        public string Method { get; set; }
        public string ApiIDSys { get; set; }
        public List<RolePermissionDto> Roles { get; set; }
    }
}
