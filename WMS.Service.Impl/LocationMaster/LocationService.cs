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
using WIM.Core.Entity.WarehouseManagement;
using WMS.Repository.Impl;
using WMS.Context;

namespace WMS.Service
{
    public class LocationService : ILocationService
    {
        private WMSDbContext proc = new WMSDbContext();
        private LocationRepository repo;

        public LocationService()
        {
            repo = new LocationRepository();
        }        

        public IEnumerable<Location_MT> GetLocations()
        {           
            return repo.Get();
        }

        public Location_MT GetLocationByLocIDSys(int id)
        {           
            Location_MT Location = repo.GetByID(id);                                  
            return Location;            
        }                      

        public int CreateLocation(Location_MT Location)
        {
            using (var scope = new TransactionScope())
            {
                Location.LocNo = proc.ProcGetNewID("LC").FirstOrDefault().Substring(0, 13);
                Location.CreatedDate = DateTime.Now;
                Location.UpdateDate = DateTime.Now;
                Location.UserUpdate = "1";
                try
                {
                    repo.Insert(Location);
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
                return Location.LocIDSys;
            }
        }

        public bool UpdateLocation(int id, Location_MT Location)
        {           
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Update(Location);
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

        public bool DeleteLocation(int id)
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
