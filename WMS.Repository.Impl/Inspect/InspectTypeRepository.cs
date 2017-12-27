using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.InspectionManagement;
using WMS.Repository.Inspect;

namespace WMS.Repository.Impl.Inspect
{
    public class InspectTypeRepository : Repository<InspectType>, IInspectTypeRepository
    {
        private WMSDbContext Db { get; set; }
        public InspectTypeRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }

    }
}
