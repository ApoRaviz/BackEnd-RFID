using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Entity.Country;
using WIM.Core.Context;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace WIM.Core.Service.Impl
{
    public class CountryService : Service, ICountryService
    {

        public CountryService()
        {
        }

        public IEnumerable<Country_MT> GetCountry()
        {
            IEnumerable<Country_MT> CountryName;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICountryRepository repo = new CountryRepository(Db);
                CountryName = repo.Get();
            }
            return CountryName;
        }

        public Country_MT GetCountryByCountryIDSys(int id)
        {
            Country_MT Country;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICountryRepository repo = new CountryRepository(Db);
                Country = repo.GetByID(id);
            }
            return Country;
        }

        public int CreateCountry(Country_MT Country)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICountryRepository repo = new CountryRepository(Db);
                        repo.Insert(Country);
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
                
                return Country.CountryIDSys;
            }
        }

        public bool UpdateCountry(Country_MT Country)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICountryRepository repo = new CountryRepository(Db);
                        repo.Update(Country);
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

        public bool DeleteCountry(int id)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICountryRepository repo = new CountryRepository(Db);
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
