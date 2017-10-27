using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.RoleAndPermission;

namespace WIM.Core.Service
{
    public interface IRoleService
    {
        IEnumerable<Role> GetRoles();
        IEnumerable<Role> GetRoles(int projectIDSys);
        Role GetRoleByLocIDSys(string id);
        Role GetRoleByName(string name);
        List<RolePermissionDto> GetRoleNotPermissionID(string id);
        string CreateRole(Role Role);
        bool UpdateRole(string id, Role Role);
        bool DeleteRole(string id);
        List<RolePermissionDto> GetRoleByPermissionID(string id);
        List<Role> GetRoleByProjectUser(int id ,string userid);
        List<Role> GetRoleByUserID(string userid );
    }
}
