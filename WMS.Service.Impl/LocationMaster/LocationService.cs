using System.Collections.Generic;
using System.Data.Entity.Validation;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Service.LocationMaster;
using WIM.Core.Common.Utility.Validation;
using System.Transactions;
using WMS.Repository.Warehouse;
using WIM.Core.Common.Utility.Helpers;
using System.Data.Entity.Infrastructure;
using WMS.Common.ValueObject;
using WMS.Repository.Impl;

namespace WMS.Service.Impl.LocationMaster
{
    public class LocationService : WIM.Core.Service.Impl.Service, ILocationService
    {
        private WMSDbContext proc = new WMSDbContext();
        //private LocationRepository repo;

        //public LocationService()
        //{
        //    repo = new LocationRepository();
        //}


        public IEnumerable<GroupLocationDto> GetList()
        {
            using (WMSDbContext db = new WMSDbContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        IGroupLocationRepository gLocationRepo = new GroupLocationRepository(db);
                        IEnumerable<GroupLocationDto> gLocation = gLocationRepo.GetList();
                        return gLocation;
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                }
            }
        }

        public GroupLocation CreateLocation(GroupLocation Location)
        {
            using (WMSDbContext db = new WMSDbContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        IGroupLocationRepository headRepo = new GroupLocationRepository(db);
                        ILocationRepository dRepo = new Repository.Impl.Location.LocationRepository(db);

                        GroupLocation LocationReq = headRepo.Insert(Location);
                        db.SaveChanges();
                        foreach (var entity in Location.Location)
                        {
                            entity.GroupLocIDSys = LocationReq.GroupLocIDSys;
                            dRepo.Insert(entity);
                        }
                        db.SaveChanges();
                        scope.Complete();
                        return LocationReq;
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                }
            }


        }

        public bool DeleteLocation(int id)
        {
            throw new System.NotImplementedException();
        }

        public GroupLocation GetLocationByLocIDSys(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<GroupLocation> GetLocations()
        {
            throw new System.NotImplementedException();
        }

        //public IEnumerable<Location_MT> GetLocations()
        //{           
        //    return repo.Get();
        //}

        //public Location_MT GetLocationByLocIDSys(int id)
        //{           
        //    Location_MT Location = repo.GetByID(id);                                  
        //    return Location;            
        //}                      

        //public int CreateLocation(Location_MT Location)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //       // Location.LocNo = proc.ProcGetNewID("LC").FirstOrDefault().Substring(0, 13);
        //        //Location.CreatedDate = DateTime.Now;
        //        //Location.UpdateDate = DateTime.Now;
        //        //Location.UserUpdate = "1";
        //        try
        //        {
        //            repo.Insert(Location);
        //            scope.Complete();
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
        //            throw ex;
        //        }
        //        return Location.LocIDSys;
        //    }
        //}

        //public bool UpdateLocation(int id, Location_MT Location)
        //{           
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            repo.Update(Location);
        //            scope.Complete();
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
        //            throw ex;
        //        }

        //        return true;
        //    }
        //}

        //public bool DeleteLocation(int id)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            repo.Delete(id);
        //            scope.Complete();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
        //            throw ex;
        //        }


        //        return true;
        //    }
        //}

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

        public bool UpdateLocation(int id, GroupLocation Location)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<GroupLocation> ILocationService.GetList()
        {
            throw new System.NotImplementedException();
        }
    }
}
