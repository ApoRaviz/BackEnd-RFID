using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;
using WMS.Common;

namespace WMS.Repository.Impl
{
    public class MenuProjectMappingRepository : IGenericRepository<MenuProjectMapping>
    {
        private CoreDbContext Db { get; set; }

        public MenuProjectMappingRepository()
        {
            Db = new CoreDbContext();
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

        public IEnumerable<MenuDto> GetMenuDtoDefault(int i)
        {
            IEnumerable<MenuDto> MenuProjectMappingdto = (from c in Db.Menu_MT
                                                          where c.IsDefault == i
                                                          select c).Select(b =>
            new MenuDto()
            {
                MenuIDSys = b.MenuIDSys,
                MenuName = b.MenuName,
                MenuParentID = b.MenuParentID,
                Sort = b.Sort,
                Url = b.Url,
                Api = b.Api,
                have = 1
            }
                ).ToList();

            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuProjectMapping> Get()
        {
            var menu = from c in Db.MenuProjectMapping
                       orderby c.MenuName
                       select c;
            return menu.ToList();
        }

        public MenuProjectMapping GetByID(object id)
        {
            var menu = from c in Db.MenuProjectMapping
                       where c.MenuIDSys.Equals(id)
                       orderby c.MenuIDSysParent, c.Sort
                       select c;
            return menu.SingleOrDefault();
        }

        public IEnumerable<MenuProjectMapping> GetByProjectID(int id)
        {
            var menu = from c in Db.MenuProjectMapping
                       where c.ProjectIDSys.Equals(id)
                       orderby c.MenuIDSysParent, c.Sort
                       select c;
            return menu.ToList();
        }

        public void Insert(MenuProjectMapping entity)
        {
            Db.MenuProjectMapping.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(MenuProjectMapping entityToDelete)
        {
            Db.MenuProjectMapping.Remove(entityToDelete);
            Db.SaveChanges();
        }

        public void Update(MenuProjectMapping entityToUpdate)
        {
            var menu = (from c in Db.MenuProjectMapping
                        where c.MenuIDSys == entityToUpdate.MenuIDSys &&
                        c.ProjectIDSys == entityToUpdate.ProjectIDSys
                        select c).SingleOrDefault();
            menu.Sort = entityToUpdate.Sort;
            menu.MenuIDSysParent = entityToUpdate.MenuIDSysParent;
            menu.MenuName = entityToUpdate.MenuName;
            menu.MenuIDSys = entityToUpdate.MenuIDSys;
            menu.ProjectIDSys = entityToUpdate.ProjectIDSys;
            Db.SaveChanges();
        }

        public IEnumerable<MenuProjectMapping> GetMany(Func<MenuProjectMapping, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<MenuProjectMapping> GetManyQueryable(Func<MenuProjectMapping, bool> where)
        {
            throw new NotImplementedException();
        }

        public MenuProjectMapping Get(Func<MenuProjectMapping, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<MenuProjectMapping, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MenuProjectMapping> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<MenuProjectMapping> GetWithInclude(Expression<Func<MenuProjectMapping, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public MenuProjectMapping GetSingle(Func<MenuProjectMapping, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public MenuProjectMapping GetFirst(Func<MenuProjectMapping, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
