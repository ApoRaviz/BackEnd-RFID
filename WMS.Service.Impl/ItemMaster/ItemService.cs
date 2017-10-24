using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WIM.Core.Repository.Impl;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class ItemService : IItemService
    {
        //private WMSDbContext db = WMSDbContext.Create();
        private ItemRepository repo;

        public ItemService()
        {
            repo = new ItemRepository();
        }

        public IEnumerable<ItemDto> GetItems()
        {
            IEnumerable<Item_MT> items = repo.Get();

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
            var query = repo.GetByID(id);

            if (tableNames != null)
            {
                foreach (string tableName in tableNames)
                {
                    switch (tableName)
                    {
                        case "ItemUnitMapping":
                            //query = query.Include(it => it.ItemUnitMapping.Select(s => s.Unit_MT));
                            break;
                        default:
                            //query = query.Include(tableName);
                            break;
                    }
                }
            }

            return Mapper.Map<Item_MT, ItemDto>(query);
        }        

        public int CreateItem(Item_MT item)
        {
            using (var scope = new TransactionScope())
            {       
                try
                {
                 repo.Insert(item);
                 scope.Complete();
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
                return item.ItemIDSys;
            }
        }

        public bool UpdateItem(int id, Item_MT item)
        {
            using (var scope = new TransactionScope())
            {
                try
                { 
                    repo.Update(item);
                    scope.Complete();
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
                return true;
            }
        }

        public bool DeleteItem(int id)
        {
            using (var scope = new TransactionScope())
            {
                
                try
                {
                    repo.Delete(id);
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
