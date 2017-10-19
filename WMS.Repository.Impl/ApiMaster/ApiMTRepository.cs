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

namespace WMS.Repository.Impl
{
    public class ApiMTRepository : IGenericRepository<Api_MT>
    {
        private CoreDbContext Db { get; set; }

        public ApiMTRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<Api_MT> Get()
        {
            var api = from c in Db.Api_MT
                      select c;
            return api;
        }

        public Api_MT GetByID(object id)
        {
            Api_MT ApiMT = (from i in Db.Api_MT
                            where i.ApiIDSys == id
                            select i).SingleOrDefault();
            return ApiMT;
        }

        public void Insert(Api_MT entity)
        {
            Db.Api_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var delete = (from c in Db.Api_MT
                          where c.ApiIDSys == id
                          select c).SingleOrDefault();
            Db.Api_MT.Remove(delete);
            Db.SaveChanges();
        }

        public void Delete(Api_MT entityToDelete)
        {
            Db.Api_MT.Remove(entityToDelete);
            Db.SaveChanges();
        }

        public void Update(Api_MT entityToUpdate)
        {
            var data = (from c in Db.Api_MT
                        where c.ApiIDSys == entityToUpdate.ApiIDSys
                        select c).SingleOrDefault();
            data.ApiMenuMappings = entityToUpdate.ApiMenuMappings;
            data.Controller = entityToUpdate.Controller;
            data.Api = entityToUpdate.Api;
            data.Method = entityToUpdate.Method;
            Db.SaveChanges();
        }

        public IEnumerable<Api_MT> GetMany(Func<Api_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Api_MT> GetManyQueryable(Func<Api_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Api_MT Get(Func<Api_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Api_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Api_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Api_MT> GetWithInclude(Expression<Func<Api_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Api_MT GetSingle(Func<Api_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Api_MT GetFirst(Func<Api_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
