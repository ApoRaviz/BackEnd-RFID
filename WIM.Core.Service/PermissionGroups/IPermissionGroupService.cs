using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.MenuManagement;

namespace WIM.Core.Service.PermissionGroups
{
    public interface IPermissionGroupService : IService
    {
        IEnumerable<PermissionGroup> GetPermissionGroup();
        PermissionGroup GetGroupByGroupIDSys(string id);
        IEnumerable<PermissionGroup> GetGroupByMenuIDSys(int id);
        bool CreateGroup(IEnumerable<PermissionGroup> PermissionGroup);
        bool CreateApi(IEnumerable<PermissionGroupApi> PermissionGroup);
        bool UpdateGroup(PermissionGroup PermissionGroup);
        bool DeleteGroup(string id);
    }
}
