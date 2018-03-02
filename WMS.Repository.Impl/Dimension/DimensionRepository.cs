using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Repository;
using WMS.Context;
using System.Security.Principal;
using WMS.Entity.Dimension;

namespace WMS.Repository.Impl
{
    public class DimensionRepository : Repository<DimensionLayout_MT>,IDimensionRepository
    {
        private WMSDbContext Db { get; set; }
        public DimensionRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
