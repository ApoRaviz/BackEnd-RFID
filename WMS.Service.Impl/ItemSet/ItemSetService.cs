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
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WMS.Repository.Impl;
using WMS.Context;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public class ItemSetService : IItemSetService
    {
        private WMSDbContext proc;
        private ItemSetRepository repo;

        public ItemSetService()
        {
            proc = new WMSDbContext();
            repo = new ItemSetRepository();
        }

        public IEnumerable<ItemSetDto> GetItemSets()
        {
            IEnumerable<ItemSet_MT> ItemSets = repo.Get();

            IEnumerable<ItemSetDto> ItemSetDtos = Mapper.Map<IEnumerable<ItemSet_MT>, IEnumerable<ItemSetDto>>(ItemSets);
            return ItemSetDtos;
        }



        /*public ItemSetDto GetItemSet(int id, string[] tableNames)
        {
            
            var query = (from i in db.ItemSet_MT
                          where i.ItemSetIDSys == id && i.Active == 1
                          select i);

            if (tableNames != null)
            {
                foreach (string tableName in tableNames)
                {
                    switch (tableName)
                    {
                        case "ItemSetUnitMapping":
                            query = query.Include(it => it.ItemSetUnitMapping.Select(s => s.Unit_MT));
                            break;
                        default:
                            query = query.Include(tableName);
                            break;
                    }
                }
            }

            return Mapper.Map<ItemSet_MT, ItemSetDto>(query.SingleOrDefault());
        }*/

        public int CreateItemSet(ItemSet_MT ItemSet)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Insert(ItemSet);
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

                return ItemSet.ItemSetIDSys;
            }
        }

        public bool UpdateItemSet(int id, ItemSet_MT ItemSet)
        {
            using (var scope = new TransactionScope())
            {
                var existedItemSet = repo.GetByID(id);
                existedItemSet.ProjectIDSys = ItemSet.ProjectIDSys;
                existedItemSet.ItemSetCode = ItemSet.ItemSetCode;
                existedItemSet.UpdateDate = DateTime.Now;
                existedItemSet.UserUpdate = "1";

                try
                {
                    repo.Update(existedItemSet);
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

        public bool DeleteItemSet(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedItemSet = repo.GetByID(id);
                existedItemSet.Active = 0;
                existedItemSet.UpdateDate = DateTime.Now;
                existedItemSet.UserUpdate = "1";
                repo.Update(existedItemSet);
                scope.Complete();
                return true;
            }
        }

        public int CreateItemSet(ItemSetDto ItemSet)
        {
            using (var scope = new TransactionScope())
            {
                ItemSet_MT item = new ItemSet_MT();
                item.ItemSetName = ItemSet.ItemSetName;
                item.ProjectIDSys = ItemSet.ProjectIDSys;
                item.LineID = ItemSet.LineID;
                item.ItemSetCode = proc.ProcGetNewID("IS").FirstOrDefault();
                item.CreatedDate = DateTime.Now;
                item.UpdateDate = DateTime.Now;
                item.UserUpdate = "1";


                try
                {
                    repo.Insert(item);
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
                return item.ItemSetIDSys;
            }
        }

        public int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp)
        {
            using (var scope = new TransactionScope())
            {
                ItemSetDetail item = new ItemSetDetail();
                try
                {
                    foreach (var c in temp)
                    {
                        item.Qty = c.Qty;
                        item.ItemIDSys = c.ItemIDSys;
                        item.ItemSetIDSys = id;
                        repo.Insert(item);
                    }
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
                return item.ItemIDSys;
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

        public ItemSetDto GetItemSet(int id)
        {
            var item = repo.GetItemSetDto(id);
            var items = repo.GetItemSetDetailDto(id);
            item.ItemSetDetail = items;
            return item;
        }

        public bool DeleteItemSetDto(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Delete(id);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateConcurrencyException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }
            }

            return true;
        }
    }
}
