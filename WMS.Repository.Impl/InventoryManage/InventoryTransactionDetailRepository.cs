﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.InventoryManagement;
using WMS.Repository.InvenoryManagement;

namespace WMS.Repository.Impl.InventoryManage
{
    public class InventoryTransactionDetailRepository : Repository<InventoryTransactionDetail>, IInventoryTransactionDetailRepository
    {
        private WMSDbContext Db { get; set; }
        public InventoryTransactionDetailRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
