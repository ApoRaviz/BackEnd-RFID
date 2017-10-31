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
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WMS.Repository.Impl;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Repository;

namespace WMS.Service
{
    public class ItemSetService : IItemSetService
    {

        public ItemSetService()
        {
        }

        public IEnumerable<ItemSetDto> GetItemSets()
        {
            IEnumerable<ItemSetDto> ItemSetDtos;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IItemSetRepository repo = new ItemSetRepository(Db);
                IEnumerable<ItemSet_MT> ItemSets = repo.Get();
                ItemSetDtos = Mapper.Map<IEnumerable<ItemSet_MT>, IEnumerable<ItemSetDto>>(ItemSets);
            }

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
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemSetRepository repo = new ItemSetRepository(Db);
                        repo.Insert(ItemSet);
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

                return ItemSet.ItemSetIDSys;
            }
        }

        public bool UpdateItemSet(ItemSet_MT ItemSet)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemSetRepository repo = new ItemSetRepository(Db);
                        repo.Update(ItemSet);
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

        public bool DeleteItemSet(int id)
        {
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    IItemSetRepository repo = new ItemSetRepository(Db);
                    repo.Delete(id);
                    Db.SaveChanges();
                    scope.Complete();
                }
                return true;
            }
        }

        public int CreateItemSet(ItemSetDto ItemSet)
        {

            using (var scope = new TransactionScope())
            {
                ItemSet_MT item = new ItemSet_MT();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemSetRepository repo = new ItemSetRepository(Db);
                        item.ItemSetName = ItemSet.ItemSetName;
                        item.ProjectIDSys = ItemSet.ProjectIDSys;
                        item.LineID = ItemSet.LineID;
                        item.ItemSetCode = Db.ProcGetNewID("IS");
                        repo.Insert(item);
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
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IRepository<ItemSetDetail> repo = new Repository<ItemSetDetail>(Db);
                        foreach (var c in temp)
                        {
                            item.Qty = c.Qty;
                            item.ItemIDSys = c.ItemIDSys;
                            item.ItemSetIDSys = id;
                            repo.Insert(item);
                        }
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
            using (WMSDbContext Db = new WMSDbContext())
            {
                IItemSetRepository repo = new ItemSetRepository(Db);
                IRepository<ItemSetDetail> repodetail = new Repository<ItemSetDetail>(Db);
                var item = repo.GetManyQueryable(c=>c.ItemSetIDSys == id).Select(b => new ItemSetDto()
                {
                    ItemSetCode = b.ItemSetCode,
                    ItemSetIDSys = b.ItemSetIDSys,
                    ItemSetName = b.ItemSetName,
                    LineID = b.LineID,
                    ProjectIDSys = b.ProjectIDSys
                }).SingleOrDefault();
                string[] include = { "Item_MT" };
                var items = repodetail.GetWithInclude(c => c.ItemSetIDSys == id , include).Select(b => new ItemSetDetailDto()
                {
                    IDSys = b.IDSys,
                    ItemCode = b.Item_MT.ItemCode,
                    ItemIDSys = b.ItemIDSys,
                    ItemName = b.Item_MT.ItemName,
                    Qty = b.Qty
                }).ToList();
                item.ItemSetDetail = items;
                return item;
            }
            
        }

        public bool DeleteItemSetDto(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IItemSetRepository repo = new ItemSetRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
