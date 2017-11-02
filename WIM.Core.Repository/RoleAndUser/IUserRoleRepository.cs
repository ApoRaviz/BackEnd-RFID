using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.UserManagement;

namespace WIM.Core.Repository
{
    public interface IUserRoleRepository : IRepository<UserRoles>
    {
        IEnumerable<RoleUserDto> GetRoleByUserID(string userid);
        UserRoleDto GetUserRoleByUserID(string id);
        RoleUserDto GetRoleUserByRoleID(string id);
    }
}
