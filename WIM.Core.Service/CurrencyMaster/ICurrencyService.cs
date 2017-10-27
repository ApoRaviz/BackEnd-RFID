using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Currency;

namespace WIM.Core.Service
{
    public interface ICurrencyService
    {
        IEnumerable<CurrencyUnit> GetCurrency();
        CurrencyUnit GetCurrencyByCurrIDSys(int id);
        int CreateCurrency(CurrencyUnit Currency, string username);
        bool UpdateCurrency(CurrencyUnit Currency, string username);
        bool DeleteCurrency(int id);
    }
}
