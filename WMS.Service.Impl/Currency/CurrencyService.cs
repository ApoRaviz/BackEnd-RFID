using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Currency;
using WMS.Context;
using WMS.Repository;


namespace WMS.Service
{
    class CurrencyService : ICurrencyService
    {
        private WMSDbContext Db = WMSDbContext.Create();
        private GenericRepository<CurrencyUnit> Repo;
        public CurrencyService()
        {
            Repo = new GenericRepository<CurrencyUnit>(Db);
        }
        public CurrencyUnit GetCurrency()
        {
            //return Db.CurrencyUnits.To
            throw new NotImplementedException();
        }
    }
}
