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
    public class MenuRepository : IGenericRepository<Menu_MT>
    {
        private CoreDbContext Db { get; set; }

        public MenuRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<Menu_MT> Get()
        {
            var menu = from c in Db.Menu_MT
                       orderby c.MenuParentID, c.Sort
                       select c;
            return menu.ToList();
        }

        public Menu_MT GetByID(object id)
        {
            Menu_MT Menu = (from c in Db.Menu_MT
                            where c.MenuIDSys.Equals(id)
                            orderby c.MenuName
                            select c).SingleOrDefault();
            return Menu;
        }

        public IEnumerable<Menu_MT> GetByMenuParentID(int id)
        {
            var existedMenu = (from c in Db.Menu_MT
                               where c.MenuParentID == id
                               select c).ToList();
            return existedMenu;
        }

        public IEnumerable<Menu_MT> GetNotHave(int projectIDSys)
        {
            var menuQuery = from row in Db.Menu_MT
                            where !(from o in Db.MenuProjectMapping
                                    where o.ProjectIDSys == projectIDSys
                                    select o.MenuIDSys)
                                    .Contains(row.MenuIDSys)
                            orderby row.MenuParentID, row.Sort
                            select row;
            return menuQuery.ToList();
        }

        public void Insert(Menu_MT entity)
        {
            Db.Menu_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Menu_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(Menu_MT entityToUpdate)
        {
            var existedMenu = (from c in Db.Menu_MT
                               where c.MenuIDSys.Equals(entityToUpdate.MenuIDSys)
                               select c).SingleOrDefault();
            existedMenu.MenuParentID = entityToUpdate.MenuParentID;
            existedMenu.MenuName = entityToUpdate.MenuName;
            existedMenu.Url = entityToUpdate.Url;
            existedMenu.Api = entityToUpdate.Api;
            existedMenu.Sort = entityToUpdate.Sort;
            Db.SaveChanges();
        }

        public IEnumerable<Menu_MT> GetMany(Func<Menu_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Menu_MT> GetManyQueryable(Func<Menu_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Menu_MT Get(Func<Menu_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Menu_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Menu_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Menu_MT> GetWithInclude(Expression<Func<Menu_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Menu_MT GetSingle(Func<Menu_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Menu_MT GetFirst(Func<Menu_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
