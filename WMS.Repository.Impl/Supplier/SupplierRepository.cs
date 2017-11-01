using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Repository;
using WMS.Context;
using System.Security.Principal;

namespace WMS.Repository.Impl
{
    public class SupplierRepository : Repository<Supplier_MT> , ISupplierRepository
    {
        private WMSDbContext Db { get; set; }
        public SupplierRepository( WMSDbContext context):base(context)
        {
            Db = context;
        }

    }
}
