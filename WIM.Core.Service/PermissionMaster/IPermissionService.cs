﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.RoleAndPermission;

namespace WIM.Core.Service
{
    public interface IPermissionService : IService
    {
        IEnumerable<Permission> GetPermissions();
        Permission GetPermissionByLocIDSys(string id);
        List<Permission> GetPermissionByProjectID(int ProjectID);
        List<Permission> GetPermissionByMenuID(int MenuIDSys,int ProjectIDSys);
        List<Permission> GetPermissionAuto(int MenuIDSys, int ProjectIDSys);
        List<Permission> GetPermissionByProjectID(int ProjectID, string UserID);
        List<Permission> GetPermissionByRoleID(string RoleID, int ProjectIDSys);
        string CreatePermission(Permission Permission);
        bool UpdatePermission(Permission Permission );
        bool DeletePermission(string id);
        bool DeleteAllInRole(string permissionID);
        string CreateRolePermission(string PermissionId, string RoleId);
        string CreateRolePermission(string RoleID , List<PermissionTree> tree);
        bool DeleteRolePermission(string PermissionId, string RoleId);
        List<PermissionTree> GetPermissionTree(int projectid);
        bool CreatePermissionByGroup(string GroupIDSys, MenuProjectMapping menu);
        bool DeletePermissionByGroup(string GroupIDSys, MenuProjectMapping menu);
    }
}
