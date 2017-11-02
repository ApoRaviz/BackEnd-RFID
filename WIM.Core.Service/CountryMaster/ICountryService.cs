using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Country;

namespace WIM.Core.Service
{
    public interface ICountryService : IService
    {
        IEnumerable<Country_MT> GetCountry();
        Country_MT GetCountryByCountryIDSys(int id);
        int CreateCountry(Country_MT Country);
        bool UpdateCountry(Country_MT Country);
        bool DeleteCountry(int id);
    }
}