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
using WIM.Core.Context;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Impl;
using WMS.Service.LocationMaster;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WMS.Repository.Impl.Location;
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

        public IEnumerable<Location_MT> GetList()
        {
            return repo.Get();
        }

        public Location_MT GetLocationByLocIDSys(int id)
        {
            Location_MT Location = repo.GetByID(id);
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
            catch (DbEntityValidationException e)
            {
            }
            catch (DbUpdateException e)
            {
                ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                throw ex;
            }

            return null;
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
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.E4012);
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
                    throw new ValidationException(ErrorEnum.E4017);
                }
                return true;
            }
        }

        public Location_MT CreateLocation(Location_MT Location)
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
                    throw new ValidationException(ErrorEnum.E4012);
                }
                return Location;
            }
        }
    }
}
