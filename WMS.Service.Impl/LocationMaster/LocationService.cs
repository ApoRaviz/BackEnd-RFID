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
using WMS.Master;

namespace WMS.Service
{
    public class LocationService : ILocationService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Location_MT> repo;

        public LocationService()
        {
            repo = new GenericRepository<Location_MT>(db);
        }        

        public IEnumerable<Location_MT> GetLocations()
        {           
            return repo.GetAll();
        }

        public Location_MT GetLocationByLocIDSys(int id)
        {           
            Location_MT Location = db.Location_MT.Find(id);                                  
            return Location;            
        }                      

        public int CreateLocation(Location_MT Location)
        {
            using (var scope = new TransactionScope())
            {
                Location.LocNo = db.ProcGetNewID("LC").FirstOrDefault().Substring(0, 13);
                Location.CreatedDate = DateTime.Now;
                Location.UpdateDate = DateTime.Now;
                Location.UserUpdate = "1";
                
                repo.Insert(Location);
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
                return Location.LocIDSys;
            }
        }

        public bool UpdateLocation(int id, Location_MT Location)
        {           
            using (var scope = new TransactionScope())
            {
                var existedLocation = repo.GetByID(id);
                existedLocation.LineID = Location.LineID;
                existedLocation.WHID = Location.WHID;
                existedLocation.QualityType = Location.QualityType;
                existedLocation.RackType = Location.RackType;
                existedLocation.Tempature = Location.Tempature;
                existedLocation.CateIDSys = Location.CateIDSys;
                existedLocation.Weight = Location.Weight;
                existedLocation.Width = Location.Width;
                existedLocation.Length = Location.Length;
                existedLocation.Height = Location.Height;
                existedLocation.UpdateDate = DateTime.Now;
                existedLocation.UserUpdate = "1";
                repo.Update(existedLocation);
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

        public bool DeleteLocation(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedLocation = repo.GetByID(id);
                existedLocation.Active = 0;
                existedLocation.UpdateDate = DateTime.Now;
                existedLocation.UserUpdate = "1";
                repo.Update(existedLocation);
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
