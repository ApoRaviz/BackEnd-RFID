﻿using System;
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

namespace WMS.Repository.Impl
{
    public class ItemRepository : Repository<Item_MT> , IItemRepository
    {
        private WMSDbContext Db { get; set; }
        public ItemRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }

        public Item_MT GetManyWithUnit(int id)
        {
            var item = Db.Item_MT.Include(it => it.ItemUnitMapping.Select(s => s.Unit_MT)).Include(a => a.ItemInspectMapping.Select(x => x.Inspect_MT)).Where(c => c.ItemIDSys == id);
            return item.SingleOrDefault();
        }
    }
}
