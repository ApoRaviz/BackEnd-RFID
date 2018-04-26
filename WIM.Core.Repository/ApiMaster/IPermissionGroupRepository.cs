using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;

namespace WIM.Core.Repository
{
    public interface IPermissionGroupRepository : IRepository<PermissionGroup>
    {
        IEnumerable<PermissionGroup> GetPermissionGroupWithInclude(int MenuIDSys);
        IEnumerable<PermissionTree> GetPermissionByGroupAndMenu(int ProjectIDSys);
    }
}
