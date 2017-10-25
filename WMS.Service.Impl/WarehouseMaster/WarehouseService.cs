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
using WMS.Repository.Impl;
using WIM.Core.Entity.WarehouseManagement;

namespace WMS.Master
{
    public class WarehouseService : IWarehouseService
    {
        private WarehouseRepository repo;

        public WarehouseService()
        {
            repo = new WarehouseRepository();
        }

        public IEnumerable<Warehouse_MT> GetWarehouses()
        {
            var WarehouseName = repo.Get();
            return WarehouseName;
        }

        public Warehouse_MT GetWarehouseByLocIDSys(int id)
        {
            Warehouse_MT Warehouse = repo.GetByID(id);
            return Warehouse;
        }

        public int CreateWarehouse(Warehouse_MT Warehouse)
        {
            using (var scope = new TransactionScope())
            {
                Warehouse.CreatedDate = DateTime.Now;
                Warehouse.UpdateDate = DateTime.Now;
                Warehouse.UserUpdate = "1";

                try
                {
                    repo.Insert(Warehouse);
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
                scope.Complete();
                return Warehouse.WHIDSys;
            }
        }

        public bool UpdateWarehouse(int id, Warehouse_MT Warehouse)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    repo.Update(Warehouse);
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
                scope.Complete();
                return true;
            }
        }

        public bool DeleteWarehouse(int id)
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
