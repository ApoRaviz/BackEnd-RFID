using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Country;
using WIM.Core.Entity.Currency;
using WIM.Core.Repository;

namespace WMS.Repository.Impl
{
    public class CountryRepository : IGenericRepository<Country_MT>
    {
        private CoreDbContext Db;

        public CountryRepository()
        {
            Db = new CoreDbContext();
        }

        public void Delete(object id)
        {
            var existedCountry = GetByID(id);
            //existedCountry.Active = 0;
            //existedCountry.UpdateDate = DateTime.Now;
            //existedCountry.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Delete(Country_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<Country_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country_MT> Get()
        {
            var country = (from i in Db.Country_MT
                           select i);
            return country;
        }

        public Country_MT Get(Func<Country_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public Country_MT GetByID(object id)
        {
            Country_MT Currency = Db.Country_MT.Find(id);
            return Currency;
        }

        public Country_MT GetFirst(Func<Country_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country_MT> GetMany(Func<Country_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Country_MT> GetManyQueryable(Func<Country_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public Country_MT GetSingle(Func<Country_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Country_MT> GetWithInclude(Expression<Func<Country_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public void Insert(Country_MT entity)
        {
            Db.Country_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Update(Country_MT Country)
        {
            var existedCountry = GetByID(Country.CountryIDSys);
            existedCountry.CountryCode = Country.CountryCode;
            existedCountry.CountryName = Country.CountryName;
            existedCountry.PhoneCode = Country.PhoneCode;
            Db.SaveChanges();
        }
    }
}

