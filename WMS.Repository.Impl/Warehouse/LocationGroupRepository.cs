using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using WMS.Repository.Warehouse;

namespace WMS.Repository.Impl.Warehouse
{
    public class LocationGroupRepository : Repository<GroupLocation>, ILocationGroupRepository
    {
        private WMSDbContext Db;

        public LocationGroupRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }

        public IEnumerable<AutocompleteLocationDto> AutocompleteLocation(string term)
        {
            var qr = (from sp in Db.Location
                      where sp.LocNo.Contains(term)
                      select new AutocompleteLocationDto
                      {
                          LocIDSys = sp.LocIDSys,
                          LocNo = sp.LocNo
                      }
            ).ToList();
            return qr;
        }
    }
}
