using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.ItemManagement;

namespace WMS.Repository.Impl.ItemManagement
{
    public class ItemRepository
    {
        private WMSDbContext Db { get; set; }
        private IGenericRepository<Item_MT> Repo { get; set; }

        public ItemRepository()
        {
            Repo = new GenericRepository<Item_MT>(Db);
        }

        public IEnumerable<ItemDto> GetItems()
        {
            IEnumerable<Item_MT> items = (from i in db.Item_MT
                                          where i.Active == 1
                                          select i).ToList();            
        }
    }
}
