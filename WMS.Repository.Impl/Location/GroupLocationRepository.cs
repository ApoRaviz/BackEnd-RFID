using WIM.Core.Repository.Impl;
using WMS.Context;
using System.Linq;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Warehouse;
using System.Collections.Generic;
using WMS.Common.ValueObject;

namespace WMS.Repository.Impl
{

    public class GroupLocationRepository : Repository<GroupLocation>, IGroupLocationRepository
    {

        private WMSDbContext Db;

        public GroupLocationRepository(WMSDbContext contex) : base(contex)
        {
            Db = contex;
        }


        public IEnumerable<GroupLocationDto> GetList()
        {
            IEnumerable<GroupLocationDto> GroupLocation = (from gl in Db.GroupLocations
                                                           join lt in Db.LocationTypes on gl.LocTypeIDSys equals lt.LocTypeIDSys
                                                           select new GroupLocationDto
                                                           {
                                                               Columns = gl.Columns,
                                                               LocTypeIDSys = gl.LocTypeIDSys,
                                                               LocTypeName = lt.Name,
                                                               Name = gl.Name,

                                                           }).ToList();
            return GroupLocation;
        }

        //    public Location_MT GetByID(object id)
        //    {
        //        var location = from c in Db.Location_MT
        //                       where c.LocIDSys== (int)id
        //                       select c;
        //        return location.SingleOrDefault();
        //    }

        //    public void Insert(Location_MT entity)
        //    {
        //        //entity.CreatedDate = DateTime.Now;
        //        //entity.UpdateDate = DateTime.Now;
        //        //entity.UserUpdate = "1";
        //        //entity.Active = 1;
        //        Db.Location_MT.Add(entity);
        //        Db.SaveChanges();
        //    }

        //    public void Delete(object id)
        //    {
        //        var existedLocation = (from c in Db.Location_MT
        //                               where c.LocIDSys== (int)id
        //                               select c).SingleOrDefault();
        //        //existedLocation.Active = 0;
        //        //existedLocation.UpdateDate = DateTime.Now;
        //        //existedLocation.UserUpdate = "1";
        //        Db.SaveChanges();
        //    }

        //    public void Delete(Location_MT entityToDelete)
        //    {
        //        var existedLocation = (from c in Db.Location_MT
        //                               where c.LocIDSys.Equals(entityToDelete.LocIDSys)
        //                               select c).SingleOrDefault();
        //        //existedLocation.Active = 0;
        //        //existedLocation.UpdateDate = DateTime.Now;
        //        //existedLocation.UserUpdate = "1";
        //        Db.SaveChanges();
        //    }

        //    public void Update(Location_MT entityToUpdate)
        //    {
        //        var existedLocation = (from c in Db.Location_MT
        //                               where c.LocIDSys.Equals(entityToUpdate.LocIDSys)
        //                               select c).SingleOrDefault();
        //        existedLocation.LineID = entityToUpdate.LineID;
        //        existedLocation.WHID = entityToUpdate.WHID;
        //        existedLocation.QualityType = entityToUpdate.QualityType;
        //        existedLocation.RackType = entityToUpdate.RackType;
        //        existedLocation.Tempature = entityToUpdate.Tempature;
        //        existedLocation.Weight = entityToUpdate.Weight;
        //        existedLocation.Width = entityToUpdate.Width;
        //        existedLocation.Length = entityToUpdate.Length;
        //        existedLocation.Height = entityToUpdate.Height;
        //        //existedLocation.UpdateDate = DateTime.Now;
        //        //existedLocation.UserUpdate = "1";
        //        Db.SaveChanges();
        //    }

        //    public IEnumerable<Location_MT> GetMany(Func<Location_MT, bool> where)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IQueryable<Location_MT> GetManyQueryable(Func<Location_MT, bool> where)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Location_MT Get(Func<Location_MT, bool> where)
        //    {
        //        return Get().Where(where).First();
        //    }

        //    public void Delete(Func<Location_MT, bool> where)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IEnumerable<Location_MT> GetAll()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IQueryable<Location_MT> GetWithInclude(Expression<Func<Location_MT, bool>> predicate, params string[] include)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public bool Exists(object primaryKey)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Location_MT GetSingle(Func<Location_MT, bool> predicate)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Location_MT GetFirst(Func<Location_MT, bool> predicate)
        //    {
        //        throw new NotImplementedException();            
        //    }

        //}
    }
}
