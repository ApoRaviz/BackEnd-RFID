using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master
{
    public interface IPermissionService
    {
        IEnumerable<Permission> GetPermissions();
        Permission GetPermissionByLocIDSys(string id);
        List<Permission> GetPermissionByProjectID(int ProjectID);
        List<Permission> GetPermissionByMenuID(int MenuIDSys,int ProjectIDSys);
        List<Permission> GetPermissionAuto(int MenuIDSys, int ProjectIDSys);
        List<Permission> GetPermissionByProjectID(int ProjectID, string UserID);
        List<Permission> GetPermissionByRoleID(string RoleID, int ProjectIDSys);
        string CreatePermission(Permission Permission);
        bool UpdatePermission(string id, Permission Permission);
        bool DeletePermission(string id);
        bool DeleteAllInRole(string permissionID);
        string CreateRolePermission(string PermissionId, string RoleId);
        bool DeleteRolePermission(string PermissionId, string RoleId);
        List<PermissionTree> GetPermissionTree(int projectid);
    }
}
