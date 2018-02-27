using WIM.Core.Context;
using WIM.Core.Repository.Impl;
using WMS.Entity.SpareField;

namespace WMS.Repository.Impl
{
    public class SpareFieldRepository : Repository<SpareField>, ISpareFieldRepository
    {
        private CoreDbContext Db { get; set; }
        public SpareFieldRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }



    }
}
