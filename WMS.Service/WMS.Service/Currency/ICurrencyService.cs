using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Master;

namespace WMS.Service
{
    public interface ICurrencyService
    {
        CurrencyUnit GetCurrency();
    }
}
