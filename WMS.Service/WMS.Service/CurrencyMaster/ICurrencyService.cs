using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{
    public interface ICurrencyService
    {
        IEnumerable<CurrencyUnit> GetCurrency();
        CurrencyUnit GetCurrencyByCurrIDSys(int id);
        int CreateCurrency(CurrencyUnit Currency);
        bool UpdateCurrency(int id, CurrencyUnit Currency);
        bool DeleteCurrency(int id);
    }
}
