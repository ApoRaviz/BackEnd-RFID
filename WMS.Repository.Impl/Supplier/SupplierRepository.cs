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

namespace WMS.Repository.Impl
{
    public class SupplierRepository : IGenericRepository<Supplier_MT>
    {
        private CoreDbContext Db { get; set; }

        public SupplierRepository()
        {
            Db = new CoreDbContext();
        }

        public IEnumerable<Supplier_MT> Get()
        {
            var Supplier = (from i in Db.Supplier_MT
                            select i);
            return Supplier.ToList();
        }

        public Supplier_MT GetByID(object id)
        {
            var Supplier = (from i in Db.Supplier_MT
                            where i.SupIDSys== (int)id
                            select i);
            return Supplier.SingleOrDefault();
        }

        public IEnumerable<Supplier_MT> GetByProjectID(int projectID)
        {
            var supplier = from row in Db.Supplier_MT
                           where row.ProjectIDSys == projectID
                           select row;
            return supplier;
        }

        public void Insert(Supplier_MT entity)
        {
            entity.SupID = Db.ProcGetNewID("SL").Substring(0, 13);
            entity.CreatedDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.UserUpdate = "1";
            entity.Active = 1;
            Db.Supplier_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var existedSupplier = this.GetByID(id);
            existedSupplier.Active = 0;
            existedSupplier.UpdateDate = DateTime.Now;
            existedSupplier.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Delete(Supplier_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(Supplier_MT entityToUpdate)
        {
            var existedSupplier = this.GetByID(entityToUpdate.SupIDSys);
            existedSupplier.ProjectIDSys = entityToUpdate.ProjectIDSys;
            existedSupplier.CompName = entityToUpdate.CompName;
            existedSupplier.Address = entityToUpdate.Address;
            existedSupplier.SubCity = entityToUpdate.SubCity;
            existedSupplier.City = entityToUpdate.City;
            existedSupplier.Province = entityToUpdate.Province;
            existedSupplier.Zipcode = entityToUpdate.Zipcode;
            existedSupplier.CountryCode = entityToUpdate.CountryCode;
            existedSupplier.ContName = entityToUpdate.ContName;
            existedSupplier.Email = entityToUpdate.Email;
            existedSupplier.TelOffice = entityToUpdate.TelOffice;
            existedSupplier.TelExt = entityToUpdate.TelExt;
            existedSupplier.Mobile = entityToUpdate.Mobile;
            existedSupplier.UpdateDate = DateTime.Now;
            existedSupplier.UserUpdate = "1";
            Db.SaveChanges();
        }

        public IEnumerable<Supplier_MT> GetMany(Func<Supplier_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Supplier_MT> GetManyQueryable(Func<Supplier_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Supplier_MT Get(Func<Supplier_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Supplier_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Supplier_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Supplier_MT> GetWithInclude(Expression<Func<Supplier_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public Supplier_MT GetSingle(Func<Supplier_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Supplier_MT GetFirst(Func<Supplier_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
