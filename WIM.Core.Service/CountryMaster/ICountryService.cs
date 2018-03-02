using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
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
        IEnumerable<AutocompleteCountryDto> AutocompleteCountry(string term);
    }
}