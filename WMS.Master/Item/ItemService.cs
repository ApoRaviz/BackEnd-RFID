using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Master;
using WMS.Master.Unit;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master
{
    public class ItemService : IItemService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Item_MT> repo;

        public ItemService()
        {
            repo = new GenericRepository<Item_MT>(db);
        }

        public IEnumerable<ItemDto> GetItems()
        {
            IEnumerable<Item_MT> items = (from i in db.Item_MT
                                          where i.Active == 1
                                          select i).ToList();

            IEnumerable<ItemDto> itemDtos = Mapper.Map<IEnumerable<Item_MT>, IEnumerable<ItemDto>>(items);
            return itemDtos;
        }

        public ItemDto GetItem(int id, string[] tableNames)
        {
            /*Item_MT item = (from i in db.Item_MT
                            where i.ItemIDSys == id && i.Active == 1
                            select i)
                            .Include(it => it.Project_MT)
                            .Include(it => it.Supplier_MT)
                            .Include(it => it.ItemUnitMapping.Select(s => s.Unit_MT))
                            .SingleOrDefault();*/

            //return Mapper.Map<Item_MT, ItemDto>(item);
            var query = (from i in db.Item_MT
                          where i.ItemIDSys == id && i.Active == 1
                          select i);

            if (tableNames != null)
            {
                foreach (string tableName in tableNames)
                {
                    switch (tableName)
                    {
                        case "ItemUnitMapping":
                            query = query.Include(it => it.ItemUnitMapping.Select(s => s.Unit_MT));
                            break;
                        default:
                            query = query.Include(tableName);
                            break;
                    }
                }
            }

            return Mapper.Map<Item_MT, ItemDto>(query.SingleOrDefault());
        }        

        public int CreateItem(Item_MT item)
        {
            using (var scope = new TransactionScope())
            {
                item.CreatedDate = DateTime.Now;
                item.UpdateDate = DateTime.Now;
                item.ActiveDateFrom = DateTime.Now;
                item.ActiveDateTo = DateTime.Now;
                item.UserUpdate = "1";

                repo.Insert(item);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException )
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return item.ItemIDSys;
            }
        }

        public bool UpdateItem(int id, Item_MT item)
        {
            using (var scope = new TransactionScope())
            {
                var existedItem = repo.GetByID(id);
                existedItem.ProjectIDSys = item.ProjectIDSys;
                existedItem.ItemCode = item.ItemCode;
                existedItem.JAN = item.JAN;
                existedItem.ScanCode = item.ScanCode;
                existedItem.ItemName = item.ItemName;
                existedItem.ItemColor = item.ItemColor;
                existedItem.Description = item.Description;
                existedItem.ExpireControl = item.ExpireControl;
                existedItem.SerialControl = item.SerialControl;
                existedItem.SerialFormat = item.SerialFormat;
                existedItem.Spare1 = item.Spare1;
                existedItem.Spare2 = item.Spare2;
                existedItem.Spare3 = item.Spare3;
                existedItem.Spare4 = item.Spare4;
                existedItem.Spare5 = item.Spare5;
                existedItem.InspectControl = item.InspectControl;
                existedItem.ExpireControl = item.ExpireControl;
                existedItem.DimensionControl = item.DimensionControl;
                existedItem.BoxControl = item.BoxControl;
                existedItem.LotControl = item.LotControl;
                existedItem.PalletControl = item.PalletControl;
                existedItem.ItemSetControl = item.ItemSetControl;
                existedItem.MiniAlert = item.MiniAlert;
                existedItem.AlertExp = item.AlertExp;
                existedItem.TaxCond = item.TaxCond;
                existedItem.TaxPerc = item.TaxPerc;
                existedItem.UpdateDate = DateTime.Now;
                existedItem.UserUpdate = "1";
                repo.Update(existedItem);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return true;
            }
        }

        public bool DeleteItem(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedItem = repo.GetByID(id);
                existedItem.Active = 0;
                existedItem.UpdateDate = DateTime.Now;
                existedItem.UserUpdate = "1";
                repo.Update(existedItem);
                try
                {
                db.SaveChanges();
                scope.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }
                return true;
            }
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

    }
}
