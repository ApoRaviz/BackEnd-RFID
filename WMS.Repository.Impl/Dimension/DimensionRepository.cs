using System;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.Dimension;

namespace WMS.Repository.Impl
{
    public class DimensionRepository : Repository<DimensionLayout_MT>,IDimensionRepository
    {
        private WMSDbContext Db { get; set; }
        public DimensionRepository(Context.WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
