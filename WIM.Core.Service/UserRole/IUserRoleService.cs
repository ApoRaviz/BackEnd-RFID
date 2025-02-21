﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.UserManagement;

namespace WIM.Core.Service
{
    public interface IUserRoleService : IService
    {
        IEnumerable<UserRoles> GetUserRoles();
        UserRoles GetUserRoleByLocIDSys(int id);
        string CreateUserRole(UserRoles UserRole );
        bool UpdateUserRole(UserRoles UserRole);
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
