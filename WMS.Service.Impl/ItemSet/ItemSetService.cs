using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using WIM.Core.Common.Validation;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WMS.Repository.Impl;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.ItemManagement;
using System.Security.Principal;
using WMS.Repository.Impl.ItemManagement;

namespace WMS.Service
{
    public class ItemSetService : IItemSetService
    {
        private WMSDbContext proc = WMSDbContext.Create();
        // private Repository.ItemSetRepository repo;
        private IItemSetRepository repoItemSet;
        private IItemSetDetailRepository repoItemSetDetail;
        private WMSDbContext Db;

        public ItemSetService()
        {

            repoItemSet = new ItemSetRepository(new WMSDbContext());
            repoItemSetDetail = new ItemSetDetailRepository(new WMSDbContext());
        }



        public int CreateItemSet(ItemSet_MT ItemSet, IIdentity identity)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        repoItemSet.Insert(ItemSet, identity);
                        ICollection<ItemSetDetail> itemSetDetail = new List<ItemSetDetail>();
                        itemSetDetail = ItemSet.ItemSetDetails;
                        repoItemSetDetail.Insert(ItemSet.ItemSetDetails, identity, ItemSet.ItemSetIDSys);
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                    catch (DbUpdateException)
                    {
                        scope.Complete();
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }

                    return ItemSet.ItemSetIDSys;
                }

            }
        }

        public bool UpdateItemSet(int id, ItemSetDto ItemSet, IIdentity identity)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        repoItemSet.Update(ItemSet, identity);
                        repoItemSetDetail.Update(ItemSet.ItemSetDetail);
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
        }

        public bool DeleteItemSet(int id)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        repoItemSet.Delete(id);
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                    catch (DbUpdateException)
                    {
                        scope.Complete();
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }

                    return true;
                }

            }
        }

    
        public int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp)
        {
            using (var scope = new TransactionScope())
            {
                ItemSetDetail item = new ItemSetDetail();
                List<ItemSetDetail> itemLIst = new List<ItemSetDetail>();
                try
                {
                    foreach (var c in temp)
                    {
                        item.Qty = c.Qty;
                        item.ItemIDSys = c.ItemIDSys;
                        item.ItemSetIDSys = id;
                        itemLIst.Add(item);
                    }
                    Db.SaveChanges();
                    //repo.Insert(itemLIst);
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
            var items = repoItemSet.GetItemSetAndDetail(id);
            return items;
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

        public IEnumerable<ItemSetDto> GetDto()
        {
            throw new NotImplementedException();
        }

        public ItemSetDto GetDtoByID(int id, IIdentity UserIden)
        {
            throw new NotImplementedException();
        }

        ItemSetDto IItemSetService.CreateItemSet(ItemSet_MT ItemSet, IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public ItemSetDetailDto CreateItemSetDetail(ItemSetDetail ItemSet, IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public ItemSetDetailDto UpdateItemSetDetail(ItemSetDetailDto ItemSet, IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public ItemSetDto UpdateItemSet(ItemSetDto ItemSet, IIdentity UserIden)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItemSet(int id, IIdentity UserIden)
        {
            throw new NotImplementedException();
        }
    }
}