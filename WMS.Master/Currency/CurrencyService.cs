﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Repository;

namespace WMS.Master.Currency
{
    class CurrencyService : ICurrencyService
    {
        private MasterContext Db = MasterContext.Create();
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
