using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

using WIM.Core.Entity.Currency;
using WIM.Core.Context;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Service.Impl
{
    public class CurrencyService : Service, ICurrencyService
    {

        public CurrencyService()
        { 
        }

        public IEnumerable<CurrencyUnit> GetCurrency()
        {
            IEnumerable<CurrencyUnit> CurrencyName;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICurrencyRepository repo = new CurrencyRepository(Db);
                string[] include = { "Country_MT" };
                CurrencyName = repo.GetWithInclude(a => a.CurrencyIDSys == a.CurrencyIDSys, include).ToList();
            }
            return CurrencyName;
        }

        public CurrencyUnit GetCurrencyByCurrIDSys(int id)
        {
            CurrencyUnit Currency;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICurrencyRepository repo = new CurrencyRepository(Db);
                Currency = repo.GetByID(id);
            }
            return Currency;
        }

        public int CreateCurrency(CurrencyUnit Currency)
        {
            CurrencyUnit Currencynew = new CurrencyUnit();
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICurrencyRepository repo = new CurrencyRepository(Db);
                        Currencynew = repo.Insert(Currency);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.E4012);
                    throw ex;
                }
                return Currencynew.CurrencyIDSys;
            }
        }

        public bool UpdateCurrency(CurrencyUnit Currency)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICurrencyRepository repo = new CurrencyRepository(Db);
                        repo.Update(Currency);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.E4012);
                    throw ex;
                }

                return true;
            }
        }

        public bool DeleteCurrency(int id)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICurrencyRepository repo = new CurrencyRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.E4017);
                    throw ex;
                }
                return true;
            }
        }
    }
}

