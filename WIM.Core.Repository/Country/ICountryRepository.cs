using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Country;

namespace WIM.Core.Repository
{
    public interface ICountryRepository : IRepository<Country_MT>
    {
       IEnumerable<AutocompleteCountryDto> AutocompleteCountry(string term);
    }

}
