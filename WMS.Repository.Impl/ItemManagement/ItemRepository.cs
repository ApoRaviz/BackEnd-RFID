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

namespace WMS.Repository.Impl
{
    public class ItemRepository : Repository<Item_MT> , IItemRepository
    {
        private WMSDbContext Db { get; set; }

        public ItemRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
