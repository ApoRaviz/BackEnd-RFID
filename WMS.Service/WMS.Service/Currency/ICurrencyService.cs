using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Currency;

namespace WMS.Service
{
    public interface ICurrencyService
    {
        CurrencyUnit GetCurrency();
    }
}
