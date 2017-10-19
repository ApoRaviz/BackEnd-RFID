using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Repository;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;

namespace WMS.Repository.Impl
{
    public class UnitRepository : IGenericRepository<Unit_MT>
    {
        private WMSDbContext Db { get; set; }

        public UnitRepository()
        {
            Db = new WMSDbContext();
        }

        public IEnumerable<Unit_MT> Get()
        {
            return Db.Unit_MT.ToList();
        }

        public Unit_MT GetByID(object id)
        {
            return Db.Unit_MT.Where(u => u.UnitIDSys== (int)id).Include(b => b.Project_MT).SingleOrDefault();
        }

        public void Insert(Unit_MT entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.UserUpdate = "1";
            Db.Unit_MT.Add(entity);
        }

        public void Delete(object id)
        {
            var existedUnit = GetByID(id);
            existedUnit.UpdateDate = DateTime.Now;
            existedUnit.UserUpdate = "1";
            existedUnit.Active = 0;
            Db.SaveChanges();
        }

        public void Delete(Unit_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(Unit_MT entityToUpdate)
        {
            var existedUnit = GetByID(entityToUpdate.UnitIDSys);
            existedUnit.UnitName = entityToUpdate.UnitName;
            existedUnit.ProjectIDSys = entityToUpdate.ProjectIDSys;
            existedUnit.UpdateDate = DateTime.Now;
            existedUnit.UserUpdate = "1";
            Db.SaveChanges();
        }

        public IEnumerable<Unit_MT> GetMany(Func<Unit_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Unit_MT> GetManyQueryable(Func<Unit_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Unit_MT Get(Func<Unit_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Unit_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Unit_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Unit_MT> GetWithInclude(Expression<Func<Unit_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Unit_MT GetSingle(Func<Unit_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Unit_MT GetFirst(Func<Unit_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
