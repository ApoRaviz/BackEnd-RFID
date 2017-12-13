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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Entity.WarehouseManagement;
using WMS.Context;
using System.Security.Principal;
using WMS.Repository.Impl;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Helpers;

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
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
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
