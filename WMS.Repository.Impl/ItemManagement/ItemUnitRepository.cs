using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.ItemManagement;

namespace WMS.Repository.Impl.ItemManagement
{
    public class ItemUnitRepository : Repository<ItemUnitMapping>, IItemUnitRepository
    {
        private WMSDbContext Db;

        public ItemUnitRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
