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

        public virtual IEnumerable<MenuProjectMappingDto> GetAllMenu(int id , IEnumerable<MenuProjectMappingDto> menu)
        {
            var MenuProjectMappingQuery = (from row in Db.MenuProjectMapping.AsEnumerable()
                                           join o in menu.AsEnumerable() on row.MenuIDSys equals o.MenuIDSys into joined
                                           from i in joined.DefaultIfEmpty()
                                           where row.ProjectIDSys == id
                                           orderby row.MenuIDSysParent, row.Sort
                                           select new MenuProjectMappingDto
                                           {
                                               MenuIDSys = row.MenuIDSys,
                                               ProjectIDSys = row.ProjectIDSys,
                                               MenuName = row.MenuName,
                                               MenuIDSysParent = row.MenuIDSysParent ,
                                               Url = i==null ? string.Empty : i.Url,
                                               Sort = row.Sort
                                           }).ToList();


            return MenuProjectMappingQuery;
        }

        public IQueryable<MenuProjectMapping> GetMenuPermission(string userid, int projectid)
        {
            var menu = (from ur in Db.UserRoles
                       join rp in Db.RolePermissions on ur.RoleID equals rp.RoleID
                       join ps in Db.Permission on rp.PermissionID equals ps.PermissionID
                       join r in Db.Role on ur.RoleID equals r.RoleID
                       join mp in Db.MenuProjectMapping on ps.MenuIDSys equals mp.MenuIDSys
                       where r.ProjectIDSys == projectid && ur.UserID == userid && mp.ProjectIDSys == projectid
                       select mp).Include(x => x.Menu_MT);
            return menu;
        }

        public IQueryable<MenuProjectMapping> GetMenuProject(int id)
        {
            var menu = (from i in Db.MenuProjectMapping
                        where i.ProjectIDSys == id
                        orderby i.MenuIDSysParent, i.Sort
                        select i).Include("Menu_MT");
           return menu;
        }

        public IEnumerable<MenuProjectMappingDto> GetAllMenuWithContext(int id, IEnumerable<MenuProjectMappingDto> menu,CoreDbContext x)
        { 
            var MenuProjectMappingQuery = (from row in x.MenuProjectMapping
                                           join o in menu.AsEnumerable() on row.MenuIDSys equals o.MenuIDSys into joined
                                           from i in joined.AsEnumerable().DefaultIfEmpty()
                                           where row.ProjectIDSys == id
                                           orderby row.MenuIDSysParent, row.Sort
                                           select row).Include(a => a.Menu_MT);
            IEnumerable <MenuProjectMappingDto> Menu = MenuProjectMappingQuery.Select(row => new MenuProjectMappingDto
                                           {
                                               MenuIDSys = row.MenuIDSys,
                                               ProjectIDSys = row.ProjectIDSys,
                                               MenuName = row.MenuName,
                                               MenuIDSysParent = row.MenuIDSysParent,
                                               Url = row==null ? String.Empty:  row.Menu_MT.Url,
                                               Sort = row.Sort
                                           }).ToList();
            return Menu;
        }


    }
}
