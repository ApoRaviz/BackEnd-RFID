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
        public PermissionRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }

        public IEnumerable<Permission> GetPermissionByUserProject(int ProjectID, string UserID)
        {
            var temp = from ur in Db.UserRoles
                       join r in Db.Role on ur.RoleID equals r.RoleID
                       join rp in Db.RolePermissions on r.RoleID equals rp.RoleID
                       join ps in Db.Permission on rp.PermissionID equals ps.PermissionID
                       where ur.UserID == UserID && r.ProjectIDSys == ProjectID
                       select ps;
            return temp;
        }

        public IEnumerable<Permission> GetPermissionHasCreated(int MenuIDSys)
        {
            var data = from i in Db.Permission
                       where i.MenuIDSys == MenuIDSys &&
                              (from j in Db.ApiMenuMapping
                              where j.Type == "M" && j.GET == true && j.MenuIDSys == MenuIDSys
                              select j.ApiIDSys).Contains(i.ApiIDSys)
                       select i;
            return data.ToList();
        }

    }
}
