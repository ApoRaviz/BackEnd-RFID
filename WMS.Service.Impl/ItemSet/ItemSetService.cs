using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Common.ValueObject;
using WMS.Repository.Impl;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WMS.Repository;
using WMS.Repository.ItemManagement;
using WMS.Repository.Impl.ItemManagement;
using System.Data.Entity;

namespace WMS.Service
{
    public class ItemSetService : WIM.Core.Service.Impl.Service, IItemSetService
    {


        public ItemSetService()
        {
        }

        public ItemSetDto GetItemSet(int id, string[] tableNames)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                IItemSetRepository repo = new ItemSetRepository(Db);
                ItemSet_MT query;

                if (tableNames != null)
                {
                    query = repo.Get(i => i.ItemSetIDSys == id && i.IsActive == true);
                }
                else
                {
                    query = repo.GetWithInclude(i => i.ItemSetIDSys == id && i.IsActive == true, tableNames).SingleOrDefault();
                }

                return Mapper.Map<ItemSet_MT, ItemSetDto>(query);
            }
        }

        public object CreateItemSet(ItemSet_MT ItemSet)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                IItemSetRepository repo = new ItemSetRepository(Db);
                IItemSetDetailRepository repodetail = new ItemSetDetailRepository(Db);
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        ItemSet_MT itemset = new ItemSet_MT();
                        ICollection<ItemSetDetail> itemDetails = ItemSet.ItemSetDetails;
                        
                        ItemSet.ItemSetCode = Db.ProcGetNewID("IS");
                        itemset = repo.Insert(ItemSet);

                        foreach(var data in itemDetails)
                        {
                            repodetail.Insert(data);
                        }

                        Db.SaveChanges();
                        scope.Complete();
                        return new { ItemSetIDSys = itemset.ItemSetIDSys,
                                    ItemSetCode = itemset.ItemSetCode};
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }

                }
            }
        }

        public bool UpdateItemSet(int id, ItemSet_MT ItemSet)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    IItemSetRepository repo = new ItemSetRepository(Db);
                    var existedItemSet = repo.GetByID(id);
                    existedItemSet.ProjectIDSys = ItemSet.ProjectIDSys;
                    existedItemSet.ItemSetCode = ItemSet.ItemSetCode;

                    try
                    {
                        repo.Update(existedItemSet);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }
                    return true;
                }
            }
        }


        public int UpdateItemSet(ItemSet_MT ItemSet)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    IItemSetDetailRepository repo = new ItemSetDetailRepository(Db);
                    IItemSetRepository repoitemset = new ItemSetRepository(Db);
                    ItemSetDetail item = new ItemSetDetail();
                    try
                    {
                        repoitemset.Update(ItemSet);
                        foreach (var data in ItemSet.ItemSetDetails)
                        {
                            data.ItemSetIDSys = ItemSet.ItemSetIDSys;
                            repo.Insert(data);

                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }
                    return ItemSet.ItemSetIDSys;
                }
            }
        }

        public bool DeleteItemSet(int id)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    IItemSetRepository repo = new ItemSetRepository(Db);
                    var existedItemSet = repo.GetByID(id);
                    repo.Update(existedItemSet);
                    Db.SaveChanges();
                    scope.Complete();

                    return true;
                }
            }
        }

        public int CreateItemSet(ItemSetDto ItemSet)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    IItemSetRepository repo = new ItemSetRepository(Db);
                    ItemSet_MT item = new ItemSet_MT();
                    item.ItemSetName = ItemSet.ItemSetName;
                    item.ProjectIDSys = ItemSet.ProjectIDSys;
                    item.LineID = ItemSet.LineID;
                    item.ItemSetCode = Db.ProcGetNewID("IS").FirstOrDefault().ToString();
                    try
                    {
                        item = repo.Insert(item);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }
                    return item.ItemSetIDSys;
                }
            }
        }

        public int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    IItemSetDetailRepository repo = new ItemSetDetailRepository(Db);
                    ItemSetDetail item = new ItemSetDetail();
                    try
                    {
                        foreach (var c in temp)
                        {
                            item.Qty = c.Qty;
                            item.ItemIDSys = c.ItemIDSys;
                            item.ItemSetIDSys = id;
                            item = repo.Insert(item);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                        throw ex;
                    }
                    return item.ItemIDSys;
                }
            }
        }

        public ItemSetDto GetItemSet(int id)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    IItemSetRepository repoItemSet = new ItemSetRepository(Db);
                    return repoItemSet.GetItemSetAndDetail(id);
                }
            }
        }

        public IEnumerable<ItemSetDto> GetDto(int limit = 50)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    IItemSetRepository repo = new ItemSetRepository(Db);
                    string[] include = { "ItemSetDetails", "ItemSetDetails.Item_MT" };
                    var item = repo.GetWithInclude(a => a.IsActive == true, include)
                               .Select(data => new ItemSetDto()
                               {
                                   ItemSetIDSys = data.ItemSetIDSys,
                                   ItemSetCode = data.ItemSetCode,
                                   ItemSetName = data.ItemSetName,
                                   LineID = data.LineID,
                                   ProjectIDSys = data.ProjectIDSys,
                                   ItemSetDetails = data.ItemSetDetails.Select(b => new ItemSetDetailDto()
                                   {
                                       ItemIDSys = b.ItemIDSys,
                                       ItemName = b.Item_MT.ItemName,
                                       ItemCode = b.Item_MT.ItemCode,
                                       Qty = b.Qty
                                   }).ToList()
                               }).ToList();
                    return item;
                }
            }
        }

        public bool DeleteItemSetDto(int id)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                try
                {
                    IItemSetDetailRepository repo = new ItemSetDetailRepository(Db);
                    var data = repo.GetMany(a => a.ItemSetIDSys == id);
                    Db.ItemSetDetail.RemoveRange(data);
                    Db.SaveChanges();
                    return true;

                }
                catch (DbEntityValidationException)
                {
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4017));
                    throw ex;
                }
                catch (DbUpdateConcurrencyException)
                {
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4017));
                    throw ex;
                }
            }
        }

        public IEnumerable<ItemSetDto> GetItemSets()
        {
            throw new NotImplementedException();
        }

    }
}