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
using WMS.Common;
using WIM.Core.Context;
using WIM.Core.Entity.SupplierManagement;
using WMS.Repository.Impl;
using WMS.Context;

namespace WMS.Service
{ 
    public class SupplierService : ISupplierService
    {
        private SupplierRepository repo;
        private WMSDbContext proc;

        public SupplierService()
        {
            repo = new SupplierRepository();
            proc = new WMSDbContext();
        }        

        public IEnumerable<Supplier_MT> GetSuppliers()
        {           
            return repo.Get();
        }

        public IEnumerable<Supplier_MT> GetSuppliersByProjectID(int projectID)
        {
            var supplier = repo.GetByProjectID(projectID);
            return supplier;
        }

        public Supplier_MT GetSupplierBySupIDSys(int id)
        {           
            Supplier_MT Supplier = repo.GetByID(id);                                  
            return Supplier;            
        }                      

        public int CreateSupplier(Supplier_MT Supplier)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Insert(Supplier);
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
          
                try
                {
                    repo.Update(supplier);
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
                try
                {
                    repo.Delete(id);
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
