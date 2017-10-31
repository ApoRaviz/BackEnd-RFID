﻿using AutoMapper;
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

using WIM.Core.Entity.Currency;
using WIM.Core.Context;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace WIM.Core.Service.Impl
{
    public class CurrencyService : ICurrencyService
    {
        private IIdentity user { get; set; }
        public CurrencyService(IIdentity identity)
        {
            user = identity;
        }

        public IEnumerable<CurrencyUnit> GetCurrency()
        {
            IEnumerable<CurrencyUnit> CurrencyName;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICurrencyRepository repo = new CurrencyRepository(Db,user);
                string[] include = { "Country_MT" };
                CurrencyName = repo.GetWithInclude(null, include);
            }
            return CurrencyName;
        }

        public CurrencyUnit GetCurrencyByCurrIDSys(int id)
        {
            CurrencyUnit Currency;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICurrencyRepository repo = new CurrencyRepository(Db,user);
                Currency = repo.GetByID(id);
            }
            return Currency;
        }

        public int CreateCurrency(CurrencyUnit Currency)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICurrencyRepository repo = new CurrencyRepository(Db,user);
                        repo.Insert(Currency);
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
                return Currency.CurrencyIDSys;
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
                        ICurrencyRepository repo = new CurrencyRepository(Db,user);
                        repo.Update(Currency);
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

        public bool DeleteCurrency(int id)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICurrencyRepository repo = new CurrencyRepository(Db,user);
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

