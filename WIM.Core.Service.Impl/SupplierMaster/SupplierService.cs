using System.Collections.Generic;
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
using WIM.Core.Common.Utility.Extensions;

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
                supplier = repo.GetMany(c => c.ProjectIDSys == projectID);
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

        public Supplier_MT CreateSupplier(Supplier_MT Supplier)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ISupplierRepository repo = new SupplierRepository(Db);
                        Supplier.SupID = Db.ProcGetNewID("SL").Substring(0, 13);
                        Supplier.ProjectIDSys = Identity.GetProjectIDSys();
                        repo.Insert(Supplier);
                        Db.SaveChanges();
                        Supplier = repo.Get(x => x.SupID == Supplier.SupID);
                        scope.Complete();   
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return Supplier;
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
                        supplier.ProjectIDSys = Identity.GetProjectIDSys();
                        repo.Update(supplier);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                        repo.Delete(x => x.SupIDSys == id && x.ProjectIDSys == Identity.GetProjectIDSys());
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                    throw ex;
                }
                return true;
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
