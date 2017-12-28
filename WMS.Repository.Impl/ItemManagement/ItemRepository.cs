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
using WMS.Context;
using WMS.Entity.ItemManagement;
using System.Security.Principal;
using WMS.Common.ValueObject;

namespace WMS.Repository.Impl
{
    public class ItemRepository : Repository<Item_MT>, IItemRepository
    {
        private WMSDbContext Db { get; set; }
        public ItemRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }

        public Item_MT GetManyWithUnit(int id)
        {
            var item = Db.Item_MT.Include(it => it.ItemUnitMapping.Select(s => s.Unit_MT)).Include(a => a.ItemInspectMapping.Select(x => x.Inspect_MT)).Include(b => b.Supplier_MT).Where(c => c.ItemIDSys == id);
            return item.SingleOrDefault();
        }

        public IEnumerable<AutocompleteItemDto> AutocompleteItem(string term)
        {
            var qr = (from ai in Db.Item_MT
                      where ai.ItemCode.Contains(term)
                      || ai.ItemName.Contains(term)
                      select new AutocompleteItemDto
                      {
                          ItemIDSys = ai.ItemIDSys,
                          ItemCode = ai.ItemCode,
                          ItemName = ai.ItemName
                      }
                     ).ToList();
            return qr;
        }
    }
}
