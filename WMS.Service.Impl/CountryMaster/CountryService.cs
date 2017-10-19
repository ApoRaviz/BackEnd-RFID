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
    public class CountryService : ICountryService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Country_MT> repo;

        public CountryService()
        {
            repo = new GenericRepository<Country_MT>(db);
        }

        public IEnumerable<Country_MT> GetCountry()
        {
            return repo.GetAll();
        }

        public Country_MT GetCountryByCountryIDSys(int id)
        {
            Country_MT Country = db.Country_MT.Find(id);
            return Country;
        }

        public int CreateCountry(Country_MT Country)
        {
            using (var scope = new TransactionScope())
            {

                repo.Insert(Country);
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
                return Country.CountryIDSys;
            }
        }

        public bool UpdateCountry(int id, Country_MT Country)
        {
            using (var scope = new TransactionScope())
            {
                var existedCountry = repo.GetByID(id);
                existedCountry.CountryCode = Country.CountryCode;
                existedCountry.CountryName = Country.CountryName;
                existedCountry.PhoneCode = Country.PhoneCode;


                repo.Update(existedCountry);
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

        public bool DeleteCountry(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedCountry = repo.GetByID(id);

                repo.Update(existedCountry);
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
