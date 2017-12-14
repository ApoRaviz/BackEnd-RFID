using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.RoleAndPermission;

namespace WIM.Core.Repository
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        IEnumerable<Permission> GetPermissionByUserProject(int ProjectID, string UserID);
        IEnumerable<Permission> GetPermissionHasCreated(int MenuIDSys);
    }
}
