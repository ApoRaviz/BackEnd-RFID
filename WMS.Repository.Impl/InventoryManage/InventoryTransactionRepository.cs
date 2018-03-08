using System;
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
    public class InventoryTransactionRepository : Repository<InventoryTransaction>, IInventoryTransactionRepository
    {
        private WMSDbContext Db { get; set; }
        public InventoryTransactionRepository(WMSDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
