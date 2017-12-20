using System;
using System.Collections.Generic;
using System.Linq;
using WMS.Context;
using WMS.Common;
using WMS.Entity.ItemManagement;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WMS.Common.ValueObject;
using System.Data.Entity;

namespace WMS.Repository.Impl
{
    public class ItemSetRepository : Repository<ItemSet_MT>, IItemSetRepository
    {
        private WMSDbContext Db;
        public ItemSetRepository(WMSDbContext Context) : base(Context)
        {
            Db = Context;
        }

        public ItemSetDto GetDtoByID(int id)
        {
            ItemSetDto ItemSets = (from itsm in Db.ItemSet_MT
                                   where itsm.ItemSetIDSys == id
                                   select new ItemSetDto
                                   {
                                       ItemSetCode = itsm.ItemSetCode,
                                       ItemSetIDSys = itsm.ItemSetIDSys,
                                       ItemSetName = itsm.ItemSetName,
                                       LineID = itsm.LineID,
                                       ProjectIDSys = itsm.ProjectIDSys
                                   }).FirstOrDefault();
            return ItemSets;
        }

        public ItemSetDto GetItemSetAndDetail(int id)
        {
            ItemSetDto ItemSets = (from itsm in Db.ItemSet_MT
                                   where itsm.ItemSetIDSys == id
                                   select itsm).Include(a => a.ItemSetDetails.Select(c => c.Item_MT)).Select(s => new ItemSetDto
                                   {
                                       ItemSetCode = s.ItemSetCode,
                                       ItemSetDetails = s.ItemSetDetails.Select(b => new ItemSetDetailDto()
                                       {
                                           ItemIDSys = b.ItemIDSys,
                                           ItemName = b.Item_MT.ItemName,
                                           ItemCode = b.Item_MT.ItemCode,
                                           Qty = b.Qty
                                       }).ToList(),
                                       ItemSetIDSys = s.ItemSetIDSys,
                                       ItemSetName = s.ItemSetName,
                                       LineID = s.LineID,
                                       ProjectIDSys = s.ProjectIDSys
                                   }).SingleOrDefault();
            return ItemSets;
        }

        public IEnumerable<ItemSetDetailDto> GetDtoItemSetDetail(int itemSetIDSys)
        {
            IEnumerable<ItemSetDetailDto> ItemSetDetail = (from itmsd in Db.ItemSetDetail
                                                           join itm in Db.Item_MT on itmsd.ItemIDSys equals itm.ItemIDSys
                                                           where itmsd.ItemSetIDSys == itemSetIDSys
                                                           select new ItemSetDetailDto
                                                           {
                                                               IDSys = itmsd.IDSys,
                                                               ItemCode = itm.ItemCode,
                                                               ItemIDSys = itmsd.ItemIDSys,
                                                               ItemName = itm.ItemName,
                                                               ItemSetIDSys = itmsd.ItemSetIDSys,
                                                               Qty = itmsd.Qty
                                                           }
                                                           ).ToList();
            return ItemSetDetail;
        }

        public IEnumerable<ItemSetDto> GetDto()
        {
            return GetDto(50);
        }

        public IEnumerable<ItemSetDto> GetDto(int limit)
        {
            IEnumerable<ItemSetDto> ItemSets = (from itsm in Db.ItemSet_MT
                                                select new ItemSetDto
                                                {
                                                    ItemSetCode = itsm.ItemSetCode,
                                                    //ItemSetDetail = GetItemSetDetail(itsm.ItemSetIDSys).ToList(),
                                                    ItemSetIDSys = itsm.ItemSetIDSys,
                                                    ItemSetName = itsm.ItemSetName,
                                                    LineID = itsm.LineID,
                                                    ProjectIDSys = itsm.ProjectIDSys
                                                }).ToList();
            return ItemSets;
        }
    }
}