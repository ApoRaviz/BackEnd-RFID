using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class PermissionRepository : Repository<Permission> , IPermissionRepository
    {
        private CoreDbContext Db { get; set; }
        private IIdentity User { get; set; }
        public PermissionRepository(CoreDbContext context,IIdentity identity):base(context,identity)
        {
            Db = context;
            User = identity;
        }

        public IEnumerable<Permission> GetPermissionByUserProject(int ProjectID, string UserID)
        {
            var temp = from ur in Db.UserRoles
                       join r in Db.Role on ur.RoleID equals r.RoleID
                       join rp in Db.RolePermission on r.RoleID equals rp.RoleID
                       join ps in Db.Permission on rp.PermissionID equals ps.PermissionID
                       where ur.UserID == UserID && r.ProjectIDSys == ProjectID
                       select ps;
            return temp;
        }

    }
}
