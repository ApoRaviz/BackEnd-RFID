using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Country;
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{
    public interface ICountryService
    {
        IEnumerable<Country_MT> GetCountry();
        Country_MT GetCountryByCountryIDSys(int id);
        int CreateCountry(Country_MT Country);
        bool UpdateCountry(int id, Country_MT Country);
        bool DeleteCountry(int id);
    }
}