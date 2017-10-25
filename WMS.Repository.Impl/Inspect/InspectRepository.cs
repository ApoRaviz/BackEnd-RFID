using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Dimension;
using WIM.Core.Repository;
using WMS.Common;
using WMS.Context;
using WMS.Entity.InspectionManagement;

namespace WMS.Repository.Impl
{
    public class InspectRepository : IGenericRepository<Inspect_MT>
    {
        private WMSDbContext Db { get; set; }

        public InspectRepository()
        {
            Db = new WMSDbContext();
        }

        public IEnumerable<Inspect_MT> Get()
        {
            return Db.Inspect_MT.ToList();
        }

        public IEnumerable<InspectType> GetType()
        {
            return Db.InspectType;
        }

        public Inspect_MT GetByID(object id)
        {
            Inspect_MT Inspect = Db.Inspect_MT.Find(id);
            return Inspect;
        }

        public void Insert(Inspect_MT entity)
        {
            Db.Inspect_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var existedInspect = GetByID(id);
            existedInspect.Active = 0;
            existedInspect.UpdateDate = DateTime.Now;
            existedInspect.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Delete(Inspect_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(Inspect_MT entityToUpdate)
        {
            var existedInspect = GetByID(entityToUpdate.InspectIDSys);
            existedInspect.InspectName = entityToUpdate.InspectName;
            existedInspect.UpdateDate = DateTime.Now;
            existedInspect.UserUpdate = "1";
            Db.SaveChanges();
        }

        public IEnumerable<Inspect_MT> GetMany(Func<Inspect_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Inspect_MT> GetManyQueryable(Func<Inspect_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Inspect_MT Get(Func<Inspect_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Inspect_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inspect_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Inspect_MT> GetWithInclude(Expression<Func<Inspect_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Inspect_MT GetSingle(Func<Inspect_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Inspect_MT GetFirst(Func<Inspect_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
