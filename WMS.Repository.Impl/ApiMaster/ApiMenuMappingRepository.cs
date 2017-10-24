using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;
using WMS.Common;

namespace WMS.Repository.Impl
{
    public class ApiMenuMappingRepository : IGenericRepository<ApiMenuMapping>
    {
        private CoreDbContext Db { get; set; }

        public ApiMenuMappingRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<ApiMenuMapping> Get()
        {
            var api = from c in Db.ApiMenuMapping
                      select c;
            return api;
        }

        public IEnumerable<ApiMenuMapping> Get(int menuid)
        {
            var api = (from c in Db.ApiMenuMapping
                       where c.MenuIDSys == menuid
                       select c).Include(b => b.Api_MT).ToList();
            return api;
        }

         public ApiMenuMapping GetByID(object id)
        {
            var api = (from c in Db.ApiMenuMapping
                       where c.ApiIDSys == id
                       select c).SingleOrDefault();
            return api;
        }

        public void Insert(ApiMenuMapping entity)
        {
            Db.ApiMenuMapping.Add(entity);
            Db.SaveChanges(); 
        }

        public void Delete(object id)
        {
            var existed = (from c in Db.ApiMenuMapping
                           where c.ApiIDSys == id
                           select c).ToList();
            Db.ApiMenuMapping.RemoveRange(existed);
            Db.SaveChanges();
        }

        public void Delete(ApiMenuMapping entityToDelete)
        {
            Db.ApiMenuMapping.Remove(entityToDelete);
            Db.SaveChanges();
        }

        public void Update(ApiMenuMapping entityToUpdate)
        {
            var existed = (from c in Db.ApiMenuMapping
                           where c.ApiIDSys == entityToUpdate.ApiIDSys && 
                           c.MenuIDSys == entityToUpdate.MenuIDSys
                           select c).SingleOrDefault();
            existed.ApiIDSys = entityToUpdate.ApiIDSys;
            existed.MenuIDSys = entityToUpdate.MenuIDSys;
            Db.SaveChanges();
        }

        public IEnumerable<ApiMenuMapping> GetMany(Func<ApiMenuMapping, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ApiMenuMapping> GetManyQueryable(Func<ApiMenuMapping, bool> where)
        {
            throw new NotImplementedException();
        }

        public ApiMenuMapping Get(Func<ApiMenuMapping, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<ApiMenuMapping, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApiMenuMapping> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ApiMenuMapping> GetWithInclude(Expression<Func<ApiMenuMapping, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public ApiMenuMapping GetSingle(Func<ApiMenuMapping, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public ApiMenuMapping GetFirst(Func<ApiMenuMapping, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
