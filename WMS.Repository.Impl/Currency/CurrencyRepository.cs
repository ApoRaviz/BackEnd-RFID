using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Currency;
using WIM.Core.Repository;

namespace WMS.Repository.Impl
{
    public class CurrencyRepository : IGenericRepository<CurrencyUnit>
    {
        private CoreDbContext Db;

        public CurrencyRepository()
        {
            Db = new CoreDbContext();
        }

        public void Delete(object id)
        {
            var existedCurrency = GetByID(id);
            existedCurrency.Active = 0;
            existedCurrency.UpdateDate = DateTime.Now;
            existedCurrency.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Delete(CurrencyUnit entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<CurrencyUnit, bool> where)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CurrencyUnit> Get()
        {
            var currency = (from i in Db.CurrencyUnit
                            select i).Include(b => b.Country_MT).ToList();
            return currency;
        }

        public CurrencyUnit Get(Func<CurrencyUnit, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CurrencyUnit> GetAll()
        {
            throw new NotImplementedException();
        }

        public CurrencyUnit GetByID(object id)
        {
            CurrencyUnit Currency = Db.CurrencyUnit.Find(id);
            return Currency;
        }

        public CurrencyUnit GetFirst(Func<CurrencyUnit, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CurrencyUnit> GetMany(Func<CurrencyUnit, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CurrencyUnit> GetManyQueryable(Func<CurrencyUnit, bool> where)
        {
            throw new NotImplementedException();
        }

        public CurrencyUnit GetSingle(Func<CurrencyUnit, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CurrencyUnit> GetWithInclude(Expression<Func<CurrencyUnit, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public void Insert(CurrencyUnit entity)
        {
            Db.CurrencyUnit.Add(entity);
            Db.SaveChanges();
        }

        public void Update(CurrencyUnit Currency)
        {
            var existedCurrency = GetByID(Currency.CurrencyIDSys);
            existedCurrency.CurrencyID = Currency.CurrencyID;
            existedCurrency.CurrencyName = Currency.CurrencyName;
            existedCurrency.Active = Currency.Active;
            existedCurrency.UpdateDate = DateTime.Now;
            existedCurrency.UserUpdate = "1";
            existedCurrency.CountryIDSys = Currency.CountryIDSys;
            Db.SaveChanges();
        }
    }
}
