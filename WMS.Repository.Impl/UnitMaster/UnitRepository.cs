using System.Collections.Generic;
using System.Linq;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Common.ValueObject;

namespace WMS.Repository.Impl
{
    public class UnitRepository : Repository<Unit_MT>, IUnitRepository
    {
        private WMSDbContext Db { get; set; }
        public UnitRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }



        public IEnumerable<AutocompleteUnitDto> AutocompleteUnit(string term)
        {
            var qr = (from um in Db.Unit_MT
                      where (um.UnitID.Contains(term)
                      || um.UnitName.Contains(term)) && um.ProjectIDSys == Identity.GetProjectIDSys()

                      select new AutocompleteUnitDto
                      {
                          UnitIDSys = um.UnitIDSys,
                          UnitID = um.UnitID,
                          UnitName = um.UnitName
                      }
                     ).ToList();
            return qr;
        }

    }
}
