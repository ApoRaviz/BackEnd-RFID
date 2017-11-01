using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Dimension;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Repository;
using WMS.Common;
using WMS.Context;
using WMS.Entity.InspectionManagement;
using System.Security.Principal;

namespace WMS.Repository.Impl
{
    public class InspectRepository : Repository<Inspect_MT> , IInspectRepository
    {
        private WMSDbContext Db { get; set; }
        private IIdentity user { get; set; }
        public InspectRepository(WMSDbContext context,IIdentity identity):base(context,identity)
        {
            Db = context;
            user = identity;
        }

    }
}
