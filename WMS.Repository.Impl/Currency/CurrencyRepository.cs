using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Currency;
using WIM.Core.Repository;

namespace WMS.Repository.Impl.Currency
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Update(CurrencyUnit entityToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
