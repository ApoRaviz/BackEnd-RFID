using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.WarehouseManagement;
using WIM.Core.Repository;
using WMS.Common;

namespace WMS.Repository.Impl
{
    public class LocationRepository : IGenericRepository<Location_MT>
    {
        private CoreDbContext Db { get; set; }

        public LocationRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<Location_MT> Get()
        {
            var location = from c in Db.Location_MT
                           select c;
            return location.ToList();
        }

        public Location_MT GetByID(object id)
        {
            var location = from c in Db.Location_MT
                           where c.LocIDSys.Equals(id)
                           select c;
            return location.SingleOrDefault();
        }

        public void Insert(Location_MT entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.UserUpdate = "1";
            entity.Active = 1;
            Db.Location_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var existedLocation = (from c in Db.Location_MT
                                   where c.LocIDSys.Equals(id)
                                   select c).SingleOrDefault();
            existedLocation.Active = 0;
            existedLocation.UpdateDate = DateTime.Now;
            existedLocation.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Delete(Location_MT entityToDelete)
        {
            var existedLocation = (from c in Db.Location_MT
                                   where c.LocIDSys.Equals(entityToDelete.LocIDSys)
                                   select c).SingleOrDefault();
            existedLocation.Active = 0;
            existedLocation.UpdateDate = DateTime.Now;
            existedLocation.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Update(Location_MT entityToUpdate)
        {
            var existedLocation = (from c in Db.Location_MT
                                   where c.LocIDSys.Equals(entityToUpdate.LocIDSys)
                                   select c).SingleOrDefault();
            existedLocation.LineID = entityToUpdate.LineID;
            existedLocation.WHID = entityToUpdate.WHID;
            existedLocation.QualityType = entityToUpdate.QualityType;
            existedLocation.RackType = entityToUpdate.RackType;
            existedLocation.Tempature = entityToUpdate.Tempature;
            existedLocation.Weight = entityToUpdate.Weight;
            existedLocation.Width = entityToUpdate.Width;
            existedLocation.Length = entityToUpdate.Length;
            existedLocation.Height = entityToUpdate.Height;
            existedLocation.UpdateDate = DateTime.Now;
            existedLocation.UserUpdate = "1";
            Db.SaveChanges();
        }

        public IEnumerable<Location_MT> GetMany(Func<Location_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Location_MT> GetManyQueryable(Func<Location_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Location_MT Get(Func<Location_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Location_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Location_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Location_MT> GetWithInclude(Expression<Func<Location_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Location_MT GetSingle(Func<Location_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Location_MT GetFirst(Func<Location_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
