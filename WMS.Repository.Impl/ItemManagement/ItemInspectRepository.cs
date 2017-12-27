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
    public class ItemInspectRepository : Repository<ItemInspectMapping>, IItemInspectRepository
    {
        private WMSDbContext Db;
        public ItemInspectRepository(WMSDbContext Context) : base(Context)
        {
            Db = Context;
        }
    }
}
