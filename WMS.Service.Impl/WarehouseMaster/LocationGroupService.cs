using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using System.Data.Entity.Infrastructure;
using WMS.Entity.WarehouseManagement;
using WMS.Context;
using WMS.Repository.Impl;
using WIM.Core.Common.Utility.Validation;
using WMS.Repository.Impl.Warehouse;
using WMS.Repository.Warehouse;
using WMS.Common.ValueObject;

namespace WMS.Master
{
    public class LocationGroupService : WIM.Core.Service.Impl.Service, ILocationGroupService
    {
        public LocationGroupService()
        {
        }

        public IEnumerable<GroupLocation> GetLocationGroup()
        {
            IEnumerable<GroupLocation> groupLocation;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ILocationGroupRepository repo = new LocationGroupRepository(Db);
                string[] include = { "LocationType", "Location" };
                groupLocation = repo.GetWithInclude(f => f.GroupLocIDSys != 0, include).ToList();
                
            }
            
            return groupLocation;
        }

        public IEnumerable<GroupLocation> GetUnassignLocationGroup()
        {
            IEnumerable<GroupLocation> groupLocation = new List<GroupLocation>();
            using (WMSDbContext Db = new WMSDbContext())
            {
                ILocationGroupRepository repo = new LocationGroupRepository(Db);
                string[] include = { "LocationType", "Location" };
                groupLocation = repo.GetWithInclude(i => i.ZoneIDSys == null
                && i.ZoneID == null
                && i.Floor == null, include).ToList();

            }

            return groupLocation;
        }

        public GroupLocation GetLocationGroupByGroupLocIDSys(int id)
        {
            GroupLocation groupLocation;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ILocationGroupRepository repo = new LocationGroupRepository(Db);
                string[] include = { "LocationType", "Location" };
                groupLocation = repo.GetWithInclude(i => i.GroupLocIDSys == id, include).SingleOrDefault();
            }
                return groupLocation;
        }

        public IEnumerable<GroupLocation> GetLocationGroupByZoneInfo(GroupLocation item)
        {
            IEnumerable<GroupLocation> groupLocation = new List<GroupLocation>();
            using (WMSDbContext Db = new WMSDbContext())
            {
                if( item != null)
                {
                    ILocationGroupRepository repo = new LocationGroupRepository(Db);
                    string[] include = { "LocationType", "Location" };
                    groupLocation = repo.GetWithInclude(i => i.GroupLocIDSys == item.GroupLocIDSys
                    && i.ZoneIDSys == item.ZoneIDSys
                    , include).ToList();
                }
            }
            return groupLocation;
        }

        public IEnumerable<GroupLocation> GetLocationGroupByZoneID(int zoneIDSys)
        {
            IEnumerable<GroupLocation> groupLocation = new List<GroupLocation>();
            using (WMSDbContext Db = new WMSDbContext())
            {
                ILocationGroupRepository repo = new LocationGroupRepository(Db);
                string[] include = { "LocationType", "Location" };
                groupLocation = repo.GetWithInclude(i => i.ZoneIDSys == zoneIDSys 
                || i.ZoneIDSys == null 
                || i.ZoneIDSys == 0
                , include).ToList();

            }
            return groupLocation;
        }

        public GroupLocation CreateLocationGroup(GroupLocation locationGroup)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        ILocationGroupRepository repoLocG = new LocationGroupRepository(Db);
                        ILocationRepository repoLoc = new LocationRepository(Db);
                        GroupLocation res = repoLocG.Insert(locationGroup);
                        foreach (Location loc in locationGroup.Location)
                        {
                            loc.GroupLocIDSys = res.GroupLocIDSys;
                            repoLoc.Insert(loc);
                        }
                        locationGroup = repoLocG.GetByID(res.GroupLocIDSys);
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
                    throw new ValidationException(ErrorEnum.E4012);
                }
                
                return locationGroup;
            }
        }

        public bool UpdateLocationGroup(int locationGroupIDSys, GroupLocation locationGroup)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        
                        ILocationGroupRepository repo = new LocationGroupRepository(Db);
                        ILocationRepository repoLoc = new LocationRepository(Db);
                        if(locationGroup != null)
                        {
                            repo.Update(locationGroup);
                            foreach (Location loc in locationGroup.Location)
                            {
                                if (loc.LocIDSys != 0)
                                {
                                    repoLoc.Update(loc);
                                }
                                else
                                {
                                    loc.GroupLocIDSys = locationGroup.GroupLocIDSys;
                                    repoLoc.Insert(loc);
                                }
                            }
                        }

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
                    throw new ValidationException(ErrorEnum.E4012);
                }
                
                return true;
            }
        }

        public bool UpdateAllLocationGroup(List<GroupLocation> locationGroups)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {

                        ILocationGroupRepository repo = new LocationGroupRepository(Db);
                        locationGroups.ForEach(f =>
                        {
                            if (f != null)
                               repo.Update(f);
                        });

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
                    throw new ValidationException(ErrorEnum.E4012);
                }

                return true;
            }
        }

        public bool DeleteLocationGroup(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        ILocationGroupRepository repo = new LocationGroupRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.E4017);
                }


                return true;
            }
        }

        //Location

        public IEnumerable<Location> GetLocation()
        {
            IEnumerable<Location> location;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ILocationZoneRepository repo = new LocationZoneRepository(Db);
                location = repo.Get();
            }

            return location;
        }

        public Location GetLocationByLocIDSys(int id)
        {
            Location location;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ILocationZoneRepository repo = new LocationZoneRepository(Db);
                location = repo.GetByID(id);
            }
            return location;
        }

        //public int CreateLocation(Location Location)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            using (WMSDbContext Db = new WMSDbContext())
        //            {
        //                ILocationZoneRepository repo = new LocationZoneRepository(Db);
        //                repo.Insert(Location);
        //                Db.SaveChanges();
        //                scope.Complete();
        //            }
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            throw new ValidationException(ErrorEnum.E4012);
        //        }

        //        return Location.LocIDSys;
        //    }
        //}

        public bool UpdateLocation(Location Location)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        ILocationZoneRepository repo = new LocationZoneRepository(Db);
                        repo.Update(Location);
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
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        ILocationZoneRepository repo = new LocationZoneRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.E4017);
                }


                return true;
            }
        }

        public IEnumerable<AutocompleteLocationDto> AutocompleteLocation(string term)
        {
            IEnumerable<AutocompleteLocationDto> autocompleteItemDto;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ILocationGroupRepository repo = new LocationGroupRepository(Db);
                autocompleteItemDto = repo.AutocompleteLocation(term);

            }
            return autocompleteItemDto;
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

        public IEnumerable<GroupLocationDto> GetListLocationGroupDto()
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        ILocationGroupRepository repo = new LocationGroupRepository(Db);
                        return repo.GetListLocationGroupDto();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.E4017);
                }
             
            }
        }
    }
}
