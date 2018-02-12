﻿using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Transactions;
using System.Data.Entity.Infrastructure;
using WIM.Core.Context;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Repository.Impl.Supplier;
using WIM.Core.Repository.Supplier;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.ValueObject;

namespace WIM.Core.Service
{ 
    public class SupplierService : WIM.Core.Service.Impl.Service, ISupplierService
    {
        public SupplierService()
        {
        }        

        public IEnumerable<Supplier_MT> GetSuppliers()
        {
            IEnumerable<Supplier_MT> supplier; 
            using (CoreDbContext Db = new CoreDbContext())
            {
                ISupplierRepository repo = new SupplierRepository(Db);
                supplier = repo.Get();
            }
            return supplier;
        }

        public IEnumerable<Supplier_MT> GetSuppliersByProjectID(int projectID)
        {
            IEnumerable<Supplier_MT> supplier;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ISupplierRepository repo = new SupplierRepository(Db);
                supplier = repo.GetMany(c=>c.ProjectIDSys == projectID);
            }
            return supplier;
        }

        public Supplier_MT GetSupplierBySupIDSys(int id)
        {
            Supplier_MT Supplier;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ISupplierRepository repo = new SupplierRepository(Db);
                Supplier = repo.GetByID(id);
            }
            return Supplier;            
        }                      

        public int CreateSupplier(Supplier_MT Supplier)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ISupplierRepository repo = new SupplierRepository(Db);
                        Supplier.SupID = Db.ProcGetNewID("SL").Substring(0, 13);
                        repo.Insert(Supplier);
                        Db.SaveChanges();
                        scope.Complete();
                     }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }
                return Supplier.SupIDSys;
            }
        }

        public bool UpdateSupplier(Supplier_MT supplier)
        {           
            using (var scope = new TransactionScope())
            {
          
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ISupplierRepository repo = new SupplierRepository(Db);
                        repo.Update(supplier);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
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
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ISupplierRepository repo = new SupplierRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4017));
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


        public IEnumerable<AutocompleteSupplierDto> AutocompleteSupplier(string term)
        {
            IEnumerable<AutocompleteSupplierDto> autocompleteItemDto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ISupplierRepository repo = new SupplierRepository(Db);
                autocompleteItemDto = repo.AutocompleteSupplier(term);

            }
            return autocompleteItemDto;
        }


    }
}
