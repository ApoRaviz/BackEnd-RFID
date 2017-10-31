using System;
using System.Security.Principal;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.ItemManagement;
using System.Collections.Generic;

namespace WMS.Repository.Impl.ItemManagement 
{
    public class ItemSetDetailRepository: Repository<ItemSetDetail>, IItemSetDetailRepository
    {
        private WMSDbContext Db;
        //private ItemSetRepository repo;


        public ItemSetDetailRepository(WMSDbContext Context) : base(Context)
    {
            Db = Context;
        }

        public ItemSetDetail CreateItemSetDetail(ItemSetDetail ItemSetDetail, IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public ItemSetDetail UpdateItemSetDetail(ItemSetDetail ItemSetDetail, IIdentity identity)
        {
            throw new NotImplementedException();
        }
    }
}