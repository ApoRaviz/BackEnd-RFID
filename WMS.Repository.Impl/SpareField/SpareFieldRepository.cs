using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.SpareField;
namespace WMS.Repository.Impl
{
    public class SpareFieldRepository : Repository<SpareField>, ISpareFieldRepository
    {
        private WMSDbContext Db { get; set; }
        public SpareFieldRepository(Context.WMSDbContext context) : base(context)
        {
            Db = context;
        }



    }
}
