
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Transactions;
using System.Data.Entity.Infrastructure;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Impl;
using WMS.Service.LocationMaster;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WMS.Repository.Warehouse;

namespace WMS.Service.Impl.LocationMaster
{
    public class LocationService : WIM.Core.Service.Impl.Service, ILocationService
    {
        private WMSDbContext proc = new WMSDbContext();
        private LocationRepository repo;

        public LocationService()
        {
            repo = new LocationRepository(proc);
        }

        public IEnumerable<Location> GetList()
        {
            return repo.Get();
        }

        public Location GetLocationByLocIDSys(int id)
        {
            Location Location = repo.GetByID(id);
            return Location;
        }

        public GroupLocation GetLocationByGroupLocIDSys(int id)
        {

            try
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    ILocationRepository repo = new LocationRepository(Db);

                    return repo.GetLocationByGroupLocIDSys(id);
                }
            }
            catch (DbEntityValidationException)
            {
            }
            catch (DbUpdateException)
            {
                ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.WRITE_DATABASE_PROBLEM));
                throw ex;
            }

            return null;
        }



        public bool UpdateLocation(int id, Location Location)
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
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    throw new ValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                }
                return true;
            }
        }

        public Location CreateLocation(Location Location)
        {
            using (var scope = new TransactionScope())
            {
                // Location.LocNo = proc.ProcGetNewID("LC").FirstOrDefault().Substring(0, 13);
                //Location.CreatedDate = DateTime.Now;
                //Location.UpdateDate = DateTime.Now;
                //Location.UserUpdate = "1";
                try
                {
                    repo.Insert(Location);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return Location;
            }
        }
    }
}
