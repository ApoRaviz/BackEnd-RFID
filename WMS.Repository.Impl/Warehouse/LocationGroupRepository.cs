using System.Collections.Generic;
using System.Linq;
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

        public LocationGroupRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<GroupLocationDto> GetListLocationGroupDto()
        {
            var qr = (from sp in Db.GroupLocation
                      join lt in Db.LocationType on sp.LocTypeIDSys equals lt.LocTypeIDSys
                      select new GroupLocationDto
                      {
                          Columns = sp.Columns,
                          Description = sp.Description,
                          GroupLocIDSys = sp.GroupLocIDSys,
                          LocTypeIDSys = sp.LocTypeIDSys,
                          LocTypeName = lt.Name,
                          Name = sp.Name,
                          Rows = sp.Rows
                      }).ToList();
            return qr;
        }

        public IEnumerable<AutocompleteLocationDto> AutocompleteLocation(string term)
        {
            var qr = (from sp in Db.Locations
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
