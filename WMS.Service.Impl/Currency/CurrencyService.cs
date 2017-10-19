using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Currency;
using WMS.Context;
using WMS.Repository;
using WMS.Repository.Impl.Currency;

namespace WMS.Service
{
    class CurrencyService : ICurrencyService
    {
        private CurrencyRepository Repo;
        public CurrencyService()
        {
            Repo = new CurrencyRepository();
        }
        public CurrencyUnit GetCurrency()
        {
            //return Db.CurrencyUnits.To
            throw new NotImplementedException();
        }
    }
}
