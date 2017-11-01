using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class MenuProjectMappingRepository : Repository<MenuProjectMapping>, IMenuProjectMappingRepository
    {
        private CoreDbContext Db { get; set; }

        public MenuProjectMappingRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }

        public IEnumerable<MenuProjectMappingDto> GetAllMenu(int id , IEnumerable<MenuProjectMappingDto> menu)
        {
            var MenuProjectMappingQuery = (from row in Db.MenuProjectMapping
                                           join o in menu on row.MenuIDSys equals o.MenuIDSys into joined
                                           from i in joined.DefaultIfEmpty()
                                           where row.ProjectIDSys == id
                                           orderby row.MenuIDSysParent, row.Sort
                                           select new MenuProjectMappingDto
                                           {
                                               MenuIDSys = row.MenuIDSys,
                                               ProjectIDSys = row.ProjectIDSys,
                                               MenuName = row.MenuName,
                                               MenuIDSysParent = row.MenuIDSysParent,
                                               Url = i.Url ?? String.Empty,
                                               Sort = row.Sort
                                           });
            return MenuProjectMappingQuery;
        }

        public IQueryable<MenuProjectMapping> GetMenuPermission(string userid, int projectid)
        {
            var menu = (from ur in Db.UserRoles
                       join rp in Db.RolePermission on ur.RoleID equals rp.RoleID
                       join ps in Db.Permission on rp.PermissionID equals ps.PermissionID
                       join r in Db.Role on ur.RoleID equals r.RoleID
                       join mp in Db.MenuProjectMapping on ps.MenuIDSys equals mp.MenuIDSys
                       where r.ProjectIDSys == projectid && ur.UserID == userid && mp.ProjectIDSys == projectid
                       select mp);
            return menu;
        }
    }
}
