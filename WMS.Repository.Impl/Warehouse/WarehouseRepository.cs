﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Repository;
using WMS.Context;
using WMS.Entity.WarehouseManagement;
using System.Security.Principal;

namespace WMS.Repository.Impl
{
    public class WarehouseRepository : Repository<Warehouse_MT> , IWarehouseRepository
    {
        private Context.WMSDbContext Db;
        public WarehouseRepository(Context.WMSDbContext context):base(context)
        {
            Db = context;
        }
    }
}
