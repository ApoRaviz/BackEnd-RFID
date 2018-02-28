using System.Collections.Generic;
using System.Linq;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.Country;

namespace WIM.Core.Repository.Impl
{
    public class CountryRepository : Repository<Country_MT> ,ICountryRepository
    {
        private CoreDbContext Db { get; set; }
        public CountryRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<AutocompleteCountryDto> AutocompleteCountry(string term)
        {
            var qr = (from ctm in Db.Country_MT
                      where ctm.CountryName.Contains(term)
                      || ctm.CountryCode.Contains(term)
                      select new AutocompleteCountryDto
                      {
                          CountryCode = ctm.CountryCode,
                          CountryIDSys = ctm.CountryIDSys,
                          CountryName = ctm.CountryName,
                      }
            ).ToList();
            return  qr;
            
        }
    }
}

