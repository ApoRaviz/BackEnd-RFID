using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.InspectionManagement;

namespace WMS.Repository.Impl
{
    public class InspectRepository : Repository<Inspect_MT> , IInspectRepository
    {
        private WMSDbContext Db { get; set; }
        public InspectRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }

    }
}
