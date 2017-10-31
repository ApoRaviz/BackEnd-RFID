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
using WIM.Core.Entity.Currency;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class CurrencyService : ICurrencyService
    {

        private CurrencyRepository repo;

        public CurrencyService()
        {
            repo = new CurrencyRepository();
        }

        public IEnumerable<CurrencyUnit> GetCurrency()
        {
            var CurrencyName = repo.Get();
            return CurrencyName;
        }

        public CurrencyUnit GetCurrencyByCurrIDSys(int id)
        {
            CurrencyUnit Currency = repo.GetByID(id);
            return Currency;
        }

        public int CreateCurrency(CurrencyUnit Currency)
        {
            using (var scope = new TransactionScope())
            {
                Currency.CreatedDate = DateTime.Now;
                Currency.UpdateDate = DateTime.Now;
                Currency.UserUpdate = "1";

                try
                {
                    repo.Insert(Currency);
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
                try
                {
                repo.Update(Currency);
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

