using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using WIM.Core.Common.Validation;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Repository.Impl;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Common.ValueObject;

namespace WMS.Service
{
    public class ItemSetService : IItemSetService
    {
        private WMSDbContext proc;
        private ItemSetRepository repo;

        public ItemSetService()
        {
            proc = new WMSDbContext();
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
            using (WMSDbContext Db = new WMSDbContext())
            {
                repo = new ItemSetRepository(Db);
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        ItemSet = repo.Insert(ItemSet);
                        scope.Complete();
                        return ItemSet.ItemSetIDSys;
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }

                }
            }
        }

        public bool UpdateItemSet(int id, ItemSet_MT ItemSet)
        {
            using (var scope = new TransactionScope())
            {
                var existedItemSet = repo.GetByID(id);
                existedItemSet.ProjectIDSys = ItemSet.ProjectIDSys;
                existedItemSet.ItemSetCode = ItemSet.ItemSetCode;

                try
                {
                    repo.Update(existedItemSet);
                    scope.Complete();
                }
                catch (DbEntityValidationException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                catch (DbUpdateException)
                {
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
                repo.Update(existedItemSet);
                scope.Complete();
                return true;
            }
        }

        public int CreateItemSet(ItemSetDto ItemSet)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                repo = new ItemSetRepository(Db);
                using (var scope = new TransactionScope())
                {
                    ItemSet_MT item = new ItemSet_MT();
                    item.ItemSetName = ItemSet.ItemSetName;
                    item.ProjectIDSys = ItemSet.ProjectIDSys;
                    item.LineID = ItemSet.LineID;
                    item.ItemSetCode = proc.ProcGetNewID("IS").FirstOrDefault().ToString();


                    try
                    {
                        repo.Insert(item);
                        scope.Complete();
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                    return item.ItemSetIDSys;
                }
            }
        }

        //public int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        ItemSetDetail item = new ItemSetDetail();
        //        try
        //        {
        //            foreach (var c in temp)
        //            {
        //                item.Qty = c.Qty;
        //                item.ItemIDSys = c.ItemIDSys;
        //                item.ItemSetIDSys = id;
        //                repo.Insert(item);
        //            }
        //            scope.Complete();
        //        }
        //        catch (DbEntityValidationException)
        //        {
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
        //            throw ex;
        //        }
        //        catch (DbUpdateException)
        //        {
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
        //            throw ex;
        //        }
        //        return item.ItemIDSys;
        //    }
        //}

        //public ItemSetDto GetItemSet(int id)
        //{
        //    repoItemSet = new ItemSetRepository(Db, Identity);
        //    return repoItemSet.GetItemSetAndDetail(id);
        //}

        //public IEnumerable<ItemSetDto> GetDto(int limit = 50)
        //{
        //    var item = repo.GetItemSetDto(id);
        //    var items = repo.GetItemSetDetailDto(id);
        //    item.ItemSetDetail = items;
        //    return item;
        //}

        public bool DeleteItemSetDto(int id)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                try
                {
                    repo = new ItemSetRepository(Db);
                    repo.Delete(id);
                    return true;

                }
                catch (DbEntityValidationException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }
                catch (DbUpdateConcurrencyException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }
            }
        }

        public IEnumerable<ItemSetDto> GetItemSets()
        {
            throw new NotImplementedException();
        }

        public ItemSetDto GetItemSet(int id)
        {
            throw new NotImplementedException();
        }

        public int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp)
        {
            throw new NotImplementedException();
        }
    }
}