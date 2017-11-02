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
using WMS.Context;
using WMS.Entity.ItemManagement;
using WIM.Core.Repository.Impl;
using WMS.Repository.Impl;
using WMS.Repository;
using System.Security.Principal;
using WMS.Common.ValueObject;

namespace WMS.Service
{
    public class ItemService : WIM.Core.Service.Impl.Service, IItemService
    {
        public ItemService()
        {
        }

        public IEnumerable<ItemDto> GetItems()
        {
            IEnumerable<ItemDto> itemDtos;
            using(WMSDbContext Db = new WMSDbContext())
            {
                IItemRepository repo = new ItemRepository(Db);
                IEnumerable<Item_MT> items = repo.Get();
                itemDtos = Mapper.Map<IEnumerable<Item_MT>, IEnumerable<ItemDto>>(items);
            }
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
            
            using (WMSDbContext Db = new WMSDbContext())
            {
                IItemRepository repo = new ItemRepository(Db);
                var query = repo.GetManyQueryable(c => c.ItemIDSys == id);

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
        }        

        public int CreateItem(Item_MT item )
        {
            using (var scope = new TransactionScope())
            {       
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemRepository repo = new ItemRepository(Db);
                        repo.Insert(item);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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

        public bool UpdateItem(Item_MT item)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemRepository repo = new ItemRepository(Db);
                        repo.Update(item);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemRepository repo = new ItemRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
