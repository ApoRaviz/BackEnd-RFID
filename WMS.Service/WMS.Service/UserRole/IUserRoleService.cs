using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Security.Entity.UserManagement;
using WMS.Common;

namespace WMS.Service
{
    public interface IUserRoleService
    {
        IEnumerable<UserRole> GetUserRoles();
        UserRole GetUserRoleByLocIDSys(int id);
        string CreateUserRole(UserRole UserRole);
        bool UpdateUserRole(int id, UserRole UserRole);
        bool DeleteUserRole(int id);
        List<RoleUserDto> GetRoleByUserID(string userid);
        List<UserRoleDto> GetUserByRoleID(string roleid);
        UserRoleDto GetUserRoleByUserID(string id);
        RoleUserDto GetRoleUserByRoleID(string id);
        bool DeleteRolePermission(string UserId, string RoleId);
        string CreateUserRoles(string userid, string roleid);
        string CreateRoleUsers(string userid, string roleid);
    }
}
