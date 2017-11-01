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
using WIM.Core.Security;

namespace WMS.Service
{
    public class ItemSetService : IItemSetService
    {
        private WMSDbContext proc = WMSDbContext.Create();
        // private Repository.ItemSetRepository repo;
        private IItemSetRepository repoItemSet;
        private IItemSetDetailRepository repoItemSetDetail;
        private WMSDbContext Db;
        private IIdentity Identity;

        public ItemSetService(IIdentity identity)
        {
            Identity = identity;
        }

        public ItemSetDto CreateItemSet(ItemSet_MT ItemSet)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                repoItemSet = new ItemSetRepository(Db, Identity);
                repoItemSetDetail = new ItemSetDetailRepository(Db, Identity);
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        ItemSet_MT itemsetMT = repoItemSet.Insert(ItemSet);
                        foreach (var entity in ItemSet.ItemSetDetails)
                        {
                            entity.ItemSetIDSys = itemsetMT.ItemSetIDSys;
                            repoItemSetDetail.Insert(entity);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                        return this.GetDtoByID(itemsetMT.ItemSetIDSys);
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
        }


        public bool UpdateItemSet(ItemSetDto ItemSet)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                repoItemSet = new ItemSetRepository(Db, Identity);
                repoItemSetDetail = new ItemSetDetailRepository(Db, Identity);
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        ItemSet_MT leaveUpdated = repoItemSet.Update(ItemSet);
                        repoItemSetDetail.Delete(t => t.ItemSetIDSys == leaveUpdated.ItemSetIDSys);
                        foreach (var entity in ItemSet.ItemSetDetail)
                        {
                            var ItemSetDetailForInsert = Mapper.Map<ItemSetDetailDto, ItemSetDetail>(entity);
                            repoItemSetDetail.Insert(ItemSetDetailForInsert);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    catch (DbEntityValidationException e)
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

        public bool DeleteItemSet(int id)
        {
            repoItemSet = new ItemSetRepository(Db, Identity);
            repoItemSetDetail = new ItemSetDetailRepository(Db, Identity);
            using (WMSDbContext Db = new WMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        repoItemSet.Delete(id);
                        scope.Complete();
                        return true;
                    }
                    catch (DbEntityValidationException e)
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

        public ItemSetDto GetItemSet(int id)
        {
            repoItemSet = new ItemSetRepository(Db, Identity);
            return repoItemSet.GetItemSetAndDetail(id);
        }

        public IEnumerable<ItemSetDto> GetDto(int limit = 50)
        {
            repoItemSet = new ItemSetRepository(Db, Identity);
            return  repoItemSet.GetDto(limit);
        }

        public ItemSetDto GetDtoByID(int id)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                try
                {
                    return repoItemSet.GetDtoByID(id);
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

        

    }
}