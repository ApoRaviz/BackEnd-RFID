
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Transactions;
using WMS.Repository;
using System.Data.Entity.Infrastructure;
using WMS.Entity.WarehouseManagement;
using WMS.Context;
using WMS.Repository.Impl;
using WIM.Core.Common.Utility.Validation;

namespace WMS.Master
{
    public class WarehouseService : WIM.Core.Service.Impl.Service, IWarehouseService
    {
        public WarehouseService()
        {
        }

        public IEnumerable<Warehouse_MT> GetWarehouses()
        {
            IEnumerable<Warehouse_MT> WarehouseName;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IWarehouseRepository repo = new WarehouseRepository(Db);
                WarehouseName = repo.Get();
            }
            
            return WarehouseName;
        }

        public Warehouse_MT GetWarehouseByLocIDSys(int id)
        {
            Warehouse_MT Warehouse;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IWarehouseRepository repo = new WarehouseRepository(Db);
                Warehouse = repo.GetByID(id);
            }
                return Warehouse;
        }

        public int CreateWarehouse(Warehouse_MT Warehouse)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IWarehouseRepository repo = new WarehouseRepository(Db);
                        repo.Insert(Warehouse);
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
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                
                return Warehouse.WHIDSys;
            }
        }

        public bool UpdateWarehouse(Warehouse_MT Warehouse )
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IWarehouseRepository repo = new WarehouseRepository(Db);
                        repo.Update(Warehouse);
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
                    throw new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                
                return true;
            }
        }

        public bool DeleteWarehouse(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IWarehouseRepository repo = new WarehouseRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    throw new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                }


                return true;
            }
        }

    }
}
