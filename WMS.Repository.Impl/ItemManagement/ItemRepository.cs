using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Repository;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;
using System.Security.Principal;

namespace WMS.Repository.Impl
{
    public class ItemRepository : Repository<Item_MT> , IItemRepository
    {
        private WMSDbContext Db { get; set; }
        private IIdentity user { get; set; }
        public ItemRepository(WMSDbContext context,IIdentity identity):base(context,identity)
        {
            Db = context;
            user = identity;
        }
    }
}
