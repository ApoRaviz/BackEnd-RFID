using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Currency;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class CurrencyRepository : Repository<CurrencyUnit>, ICurrencyRepository
    {
        private CoreDbContext Db;
        public CurrencyRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
