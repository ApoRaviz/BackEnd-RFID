using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.ItemManagement;

namespace WMS.Repository.Impl
{
    public class ItemRepository : Repository<ItemSet_MT>, IItemSetRepository
    {
        private WMSDbContext Db;

        public ItemRepository(WMSDbContext context) : base(context)
        {
            this.Db = context;
        }


        public IEnumerable<Item_MT> Get()
        {
            IEnumerable<Item_MT> items = (from i in Db.Item_MT
                                          where i.Active == 1
                                          select i).ToList();
            return items;
        }

        public Item_MT GetByID(object id)
        {
            var query = (from i in Db.Item_MT
                         where i.ItemIDSys == (int)id && i.Active == 1
                         select i).Include(b => b.ItemUnitMapping).SingleOrDefault();
            return query;
        }

        public void Insert(Item_MT entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.ActiveDateFrom = DateTime.Now;
            entity.ActiveDateTo = DateTime.Now;
            entity.UserUpdate = "1";
            entity.Active = 1;
            Db.Item_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(object id)
        {
            var existedItem = (from i in Db.Item_MT
                               where i.ItemIDSys == (int)id
                               select i).SingleOrDefault();
            existedItem.UpdateDate = DateTime.Now;
            existedItem.UserUpdate = "1";
            existedItem.Active = 0;
            Db.SaveChanges();
        }

        public void Delete(Item_MT entityToDelete)
        {
            var existedItem = (from i in Db.Item_MT
                               where i.ItemIDSys.Equals(entityToDelete.ItemIDSys)
                               select i).SingleOrDefault();
            existedItem.UpdateDate = DateTime.Now;
            existedItem.UserUpdate = "1";
            existedItem.Active = 0;
            Db.SaveChanges();
        }

        public void Update(Item_MT entityToUpdate)
        {
            var existedItem = (from i in Db.Item_MT
                               where i.ItemIDSys.Equals(entityToUpdate.ItemIDSys)
                               select i).SingleOrDefault();
            existedItem.ProjectIDSys = entityToUpdate.ProjectIDSys;
            existedItem.ItemCode = entityToUpdate.ItemCode;
            existedItem.JAN = entityToUpdate.JAN;
            existedItem.ScanCode = entityToUpdate.ScanCode;
            existedItem.ItemName = entityToUpdate.ItemName;
            existedItem.ItemColor = entityToUpdate.ItemColor;
            existedItem.Description = entityToUpdate.Description;
            existedItem.ExpireControl = entityToUpdate.ExpireControl;
            existedItem.SerialControl = entityToUpdate.SerialControl;
            existedItem.SerialFormat = entityToUpdate.SerialFormat;
            existedItem.Spare1 = entityToUpdate.Spare1;
            existedItem.Spare2 = entityToUpdate.Spare2;
            existedItem.Spare3 = entityToUpdate.Spare3;
            existedItem.Spare4 = entityToUpdate.Spare4;
            existedItem.Spare5 = entityToUpdate.Spare5;
            existedItem.InspectControl = entityToUpdate.InspectControl;
            existedItem.ExpireControl = entityToUpdate.ExpireControl;
            existedItem.DimensionControl = entityToUpdate.DimensionControl;
            existedItem.BoxControl = entityToUpdate.BoxControl;
            existedItem.LotControl = entityToUpdate.LotControl;
            existedItem.PalletControl = entityToUpdate.PalletControl;
            existedItem.ItemSetControl = entityToUpdate.ItemSetControl;
            existedItem.MiniAlert = entityToUpdate.MiniAlert;
            existedItem.AlertExp = entityToUpdate.AlertExp;
            existedItem.TaxCond = entityToUpdate.TaxCond;
            existedItem.TaxPerc = entityToUpdate.TaxPerc;
            existedItem.UpdateDate = DateTime.Now;
            existedItem.UserUpdate = "1";
            existedItem.Active = entityToUpdate.Active;
            Db.SaveChanges();
        }

        public ItemSet_MT CreateItemSet(ItemSet_MT ItemSet, IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public ItemSet_MT UpdateItemSet(ItemSetDto ItemSet, IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public ItemSetDto GetDtoByID(int id)
        {
            throw new NotImplementedException();
        }

        public ItemSetDto GetItemSetAndDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemSetDetailDto> GetDtoItemSetDetail(int itemSetIDSys)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemSetDto> GetDto()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemSetDto> GetDto(int limit)
        {
            throw new NotImplementedException();
        }
    }
}
