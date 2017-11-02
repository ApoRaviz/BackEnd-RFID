using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.RoleAndPermission;

namespace WIM.Core.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        string GetByUserAndProject(string UserID, int ProjectIDSys);
        List<Role> GetByUser(string UserID);
    }
}
