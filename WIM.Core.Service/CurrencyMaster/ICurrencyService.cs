using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Currency;

namespace WIM.Core.Service
{
    public interface ICurrencyService : IService
    {
        IEnumerable<CurrencyUnit> GetCurrency();
        CurrencyUnit GetCurrencyByCurrIDSys(int id);
        int CreateCurrency(CurrencyUnit Currency);
        bool UpdateCurrency(CurrencyUnit Currency);
        bool DeleteCurrency(int id);
    }
}
