using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Repository;
using WMS.Context;
using WMS.Entity.WarehouseManagement;

namespace WMS.Repository.Impl
{
    public class WarehouseRepository : IGenericRepository<Warehouse_MT>
    {
        private WMSDbContext Db;

        public WarehouseRepository()
        {
            Db = new WMSDbContext();
        }

        public void Delete(object id)
        {
            var existedWarehouse = GetByID(id);
            existedWarehouse.Active = 0;
            existedWarehouse.UpdateDate = DateTime.Now;
            existedWarehouse.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Delete(Warehouse_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Warehouse_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Warehouse_MT> Get()
        {
            var warehouse = (from i in Db.Warehouse_MT
                            select i);
            return warehouse;
        }

        public Warehouse_MT Get(Func<Warehouse_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Warehouse_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public Warehouse_MT GetByID(object id)
        {
            Warehouse_MT Warehouse = Db.Warehouse_MT.Find(id);
            return Warehouse;
        }

        public Warehouse_MT GetFirst(Func<Warehouse_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Warehouse_MT> GetMany(Func<Warehouse_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Warehouse_MT> GetManyQueryable(Func<Warehouse_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Warehouse_MT GetSingle(Func<Warehouse_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Warehouse_MT> GetWithInclude(Expression<Func<Warehouse_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public void Insert(Warehouse_MT entity)
        {
            Db.Warehouse_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Update(Warehouse_MT Warehouse)
        {
            var existedWarehouse = GetByID(Warehouse.WHIDSys);
            existedWarehouse.WHID = Warehouse.WHID;
            existedWarehouse.WHName = Warehouse.WHName;
            existedWarehouse.Size = Warehouse.Size;
            existedWarehouse.Address = Warehouse.Address;
            existedWarehouse.SubCity = Warehouse.SubCity;
            existedWarehouse.City = Warehouse.City;
            existedWarehouse.Province = Warehouse.Province;
            existedWarehouse.Zipcode = Warehouse.Zipcode;
            existedWarehouse.CountryCode = Warehouse.CountryCode;
            existedWarehouse.UpdateDate = DateTime.Now;
            existedWarehouse.UserUpdate = "1";
            Db.SaveChanges();
        }
    }
}
