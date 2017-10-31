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
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;
using System.Security.Principal;

namespace WMS.Repository.Impl
{
    public class ItemSetRepository : Repository<ItemSet_MT>, IItemSetRepository
    {
        private WMSDbContext Db { get; set; }
        private IIdentity user { get; set; }
        public ItemSetRepository(WMSDbContext context,IIdentity identity) : base(context,identity)
        {
            Db = context;
            user = identity;
        }

    }
}
