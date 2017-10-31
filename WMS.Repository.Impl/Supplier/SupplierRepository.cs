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
using WIM.Core.Repository.Impl;
using WMS.Repository;
using WMS.Common;
using WMS.Context;

namespace WMS.Repository.Impl
{
    public class SupplierRepository : Repository<Supplier_MT> , ISupplierRepository
    {
        private WMSDbContext Db { get; set; }

        public SupplierRepository( WMSDbContext context):base(context)
        {
            Db = context;
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

            Db.Supplier_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var existedSupplier = this.GetByID(id);
            //existedSupplier.Active = 0;
            Db.SaveChanges();
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
            Db.SaveChanges();
        }

    }
}
