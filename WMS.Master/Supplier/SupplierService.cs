using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master { 
    public class SupplierService : ISupplierService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Supplier_MT> repo;

        public SupplierService()
        {
            repo = new GenericRepository<Supplier_MT>(db);
        }        

        public IEnumerable<Supplier_MT> GetSuppliers()
        {           
            return repo.GetAll();
        }

        public IEnumerable<Supplier_MT> GetSuppliersByProjectID(int projectID)
        {
            var supplier = from row in db.Supplier_MT
                           where row.ProjectIDSys == projectID
                           select row;
            return supplier;
        }

        public Supplier_MT GetSupplierBySupIDSys(int id)
        {           
            Supplier_MT Supplier = db.Supplier_MT.Find(id);                                  
            return Supplier;            
        }                      

        public int CreateSupplier(Supplier_MT Supplier)
        {
            using (var scope = new TransactionScope())
            {
                Supplier.SupID = db.ProcGetNewID("SL").FirstOrDefault().Substring(0, 13);
                Supplier.CreatedDate = DateTime.Now;
                Supplier.UpdateDate = DateTime.Now;
                Supplier.UserUpdate = "1";
                repo.Insert(Supplier);
                try
                {
                    db.SaveChanges();
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return Supplier.SupIDSys;
            }
        }

        public bool UpdateSupplier(int id, Supplier_MT supplier)
        {           
            using (var scope = new TransactionScope())
            {
                var existedSupplier = repo.GetByID(id);
                existedSupplier.ProjectIDSys = supplier.ProjectIDSys;
                existedSupplier.CompName = supplier.CompName;
                existedSupplier.Address = supplier.Address;
                existedSupplier.SubCity = supplier.SubCity;
                existedSupplier.City = supplier.City;
                existedSupplier.Province = supplier.Province;
                existedSupplier.Zipcode = supplier.Zipcode;
                existedSupplier.CountryCode = supplier.CountryCode;
                existedSupplier.ContName = supplier.ContName;
                existedSupplier.Email = supplier.Email;
                existedSupplier.TelOffice = supplier.TelOffice;
                existedSupplier.TelExt = supplier.TelExt;
                existedSupplier.Mobile = supplier.Mobile;
                existedSupplier.UpdateDate = DateTime.Now;
                existedSupplier.UserUpdate = "1";
                repo.Update(existedSupplier);
                try
                {
                    db.SaveChanges();
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return true;
            }
        }

        public bool DeleteSupplier(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedSupplier = repo.GetByID(id);
                existedSupplier.Active = 0;
                existedSupplier.UpdateDate = DateTime.Now;
                existedSupplier.UserUpdate = "1";
                repo.Update(existedSupplier);
                try
                {
                db.SaveChanges();
                scope.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }


                return true;
            }
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

    }
}
