using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.MenuManagement;

namespace WIM.Core.Service.PermissionGroups
{
    public interface IPermissionGroupApiService : IService
    {
        IEnumerable<PermissionGroupApi> GetPermissionGroup();
        IEnumerable<PermissionGroupApi> GetGroupApiByGroupIDSys(string id);
        bool CreateGroup(IEnumerable<PermissionGroupApi> PermissionGroup);
        bool UpdateGroup(IEnumerable<PermissionGroupApi> PermissionGroup);
        bool DeleteGroup(string id);
    }
}
