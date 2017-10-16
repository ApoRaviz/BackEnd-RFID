using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.ItemManagement;

namespace WMS.Repository.Impl.ItemManagement
{
    public class ItemRepository : IItemRepository
    {
        private WMSDbContext Db { get; set; }

        public ItemRepository()
        {
            Db = new WMSDbContext();
        }

        public IEnumerable<ItemDto> GetItems()
        {
            IEnumerable<ItemDto> items = (from i in Db.Item_MT
                                          where i.Active == 1
                                          select new ItemDto
                                          {
                                               ItemName = i.ItemName
                                          }).ToList();
            return items;
        }
    }
}
