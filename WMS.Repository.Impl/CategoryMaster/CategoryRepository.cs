using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;

namespace WMS.Repository.Impl
{
    public class CategoryRepository : IGenericRepository<Category_MT>
    {
        private WMSDbContext Db { get; set; }

        public CategoryRepository()
        {
            Db = new WMSDbContext();
        }



        public IEnumerable<Category_MT> GetByProjectID(int id)
        {
            IEnumerable<Category_MT> categorys = (from i in Db.Category_MT
                                                  where i.Active == 1 && i.ProjectIDSys == id
                                                  select i).ToList();
            return categorys;
        }


        public IEnumerable<Category_MT> Get()
        {
            var categories = (from c in Db.Category_MT
                              where c.Active == 1
                              select c).ToList();
            return categories;
        }

        public Category_MT GetByID(object id)
        {
            var categories = (from i in Db.Category_MT
                              where i.CateIDSys== (int)id && i.Active == 1
                              select i).SingleOrDefault();
            return categories;
        }

        public void Insert(Category_MT entity)
        {
            entity.CateID = Db.ProcGetNewID("CT");
            entity.Active = 1;
            entity.CreatedDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.UserUpdate = "1";
            Db.Category_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            Category_MT existedCategory = (from i in Db.Category_MT
                                           where i.CateIDSys== (int)id
                                           select i).SingleOrDefault();
            existedCategory.Active = 0;
            Db.SaveChanges();
        }

        public void Delete(Category_MT entityToDelete)
        {
            Category_MT existedCategory = (from i in Db.Category_MT
                                           where i.CateIDSys == entityToDelete.CateIDSys
                                           select i).SingleOrDefault();
            existedCategory.Active = 0;
            Db.SaveChanges();
        }

        public void Update(Category_MT entityToUpdate)
        {
            Category_MT existedCategory = (from i in Db.Category_MT
                                           where i.CateIDSys == entityToUpdate.CateIDSys
                                           select i).SingleOrDefault();
            existedCategory.CateName = entityToUpdate.CateName;
            existedCategory.Active = entityToUpdate.Active;
            existedCategory.CateID = entityToUpdate.CateID;
            existedCategory.UpdateDate = DateTime.Now;
            existedCategory.UserUpdate = "1";
            Db.SaveChanges();
        }

        public IEnumerable<Category_MT> GetMany(Func<Category_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Category_MT> GetManyQueryable(Func<Category_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Category_MT Get(Func<Category_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Category_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Category_MT> GetWithInclude(Expression<Func<Category_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Category_MT GetSingle(Func<Category_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Category_MT GetFirst(Func<Category_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }

}
