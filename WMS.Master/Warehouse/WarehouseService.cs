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

namespace WMS.Master
{
    public class WarehouseService : IWarehouseService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Warehouse_MT> repo;

        public WarehouseService()
        {
            repo = new GenericRepository<Warehouse_MT>(db);
        }        

        public IEnumerable<Warehouse_MT> GetWarehouses()
        {           
            return repo.GetAll();
        }

        public Warehouse_MT GetWarehouseByLocIDSys(int id)
        {           
            Warehouse_MT Warehouse = db.Warehouse_MT.Find(id);                                  
            return Warehouse;            
        }                      

        public int CreateWarehouse(Warehouse_MT Warehouse)
        {
            using (var scope = new TransactionScope())
            {
                
                Warehouse.CreatedDate = DateTime.Now;
                Warehouse.UpdateDate = DateTime.Now;
                Warehouse.UserUpdate = "1";
                
                repo.Insert(Warehouse);
                try
                {
                    db.SaveChanges();
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
                var existedWarehouse = repo.GetByID(id);
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
                repo.Update(existedWarehouse);
                try
                {
                    db.SaveChanges();
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
                var existedWarehouse = repo.GetByID(id);
                existedWarehouse.Active = 0;
                existedWarehouse.UpdateDate = DateTime.Now;
                existedWarehouse.UserUpdate = "1";
                repo.Update(existedWarehouse);
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
