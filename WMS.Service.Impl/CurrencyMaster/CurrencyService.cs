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
    public class CurrencyService : ICurrencyService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<CurrencyUnit> repo;

        public CurrencyService()
        {
            repo = new GenericRepository<CurrencyUnit>(db);
        }

        public IEnumerable<CurrencyUnit> GetCurrency()
        {
            var CountryName = (from i in db.CurrencyUnits
                                    select i).Include(b => b.Country_MT).ToList();
            return CountryName;
        }

        public CurrencyUnit GetCurrencyByCurrIDSys(int id)
        {
            CurrencyUnit Currency = db.CurrencyUnits.Find(id);
            return Currency;
        }

        public int CreateCurrency(CurrencyUnit Currency)
        {
            using (var scope = new TransactionScope())
            {
                Currency.CreatedDate = DateTime.Now;
                Currency.UpdateDate = DateTime.Now;
                Currency.UserUpdate = "1";

                repo.Insert(Currency);
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
                return Currency.CurrencyIDSys;
            }
        }

        public bool UpdateCurrency(int id, CurrencyUnit Currency)
        {
            using (var scope = new TransactionScope())
            {
                var existedCurrency = repo.GetByID(id);
                existedCurrency.CurrencyID = Currency.CurrencyID;
                existedCurrency.CurrencyName = Currency.CurrencyName;
                existedCurrency.Active = Currency.Active;
                existedCurrency.UpdateDate = DateTime.Now;
                existedCurrency.UserUpdate = "1";
                existedCurrency.CountryIDSys = Currency.CountryIDSys;


                repo.Update(existedCurrency);
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

        public bool DeleteCurrency(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedCurrency = repo.GetByID(id);
                existedCurrency.Active = 0;
                existedCurrency.UpdateDate = DateTime.Now;
                existedCurrency.UserUpdate = "1";
                repo.Update(existedCurrency);
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

