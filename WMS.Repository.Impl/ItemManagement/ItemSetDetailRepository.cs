using System;
using System.Security.Principal;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.ItemManagement;
using System.Collections.Generic;
using WIM.Repository.ItemManagement;

namespace WMS.Repository.Impl.ItemManagement 
{
    public class ItemSetDetailRepository: Repository<ItemSetDetail>, IItemSetDetailRepository
    {
        private WMSDbContext Db;
        //private ItemSetRepository repo;


        public ItemSetDetailRepository(WMSDbContext context) : base(context)
    {
            Db = context;
        }

        public ItemSetDetail CreateItemSetDetail(ItemSetDetail ItemSetDetail, IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemSetDetail> CreateItemSetDetail(IEnumerable<ItemSetDetail> ItemSetDetail, IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public ItemSetDetail UpdateItemSetDetail(ItemSetDetail ItemSetDetail, IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemSetDetail> UpdateItemSetDetail(IEnumerable<ItemSetDetail> ItemSetDetail, IIdentity identity)
        {
            throw new NotImplementedException();
        }
    }
}