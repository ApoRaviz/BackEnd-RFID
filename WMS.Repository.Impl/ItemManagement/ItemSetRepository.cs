using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WMS.Context;
using WIM.Core.Repository;
using WMS.Common;
using WMS.Entity.ItemManagement;
using System.Transactions;
using WIM.Core.Repository.Impl;
using WMS.Repository.ItemManagement;

namespace WMS.Repository.Impl
{
    public class ItemSetRepository : Repository<ItemSet_MT>, IItemSetRepository
    {
        //private WMSDbContext proc;
        //private ItemSetRepository repo;

        private WMSDbContext Db { get; set; }

        public ItemSetRepository()
        {
            Db = new WMSDbContext();
        }





        public ItemSetDto GetByID(int id)
        {
            ItemSetDto ItemSets = (from itsm in Db.ItemSet_MT
                                   where itsm.ItemSetIDSys == id
                                   select new ItemSetDto
                                   {
                                       Active = itsm.Active,
                                       ItemSetCode = itsm.ItemSetCode,
                                       ItemSetIDSys = itsm.ItemSetIDSys,
                                       ItemSetName = itsm.ItemSetName,
                                       LineID = itsm.LineID,
                                       ProjectIDSys = itsm.ProjectIDSys,
                                       UserUpdate = itsm.UserUpdate
                                   }).FirstOrDefault();
            return ItemSets;
        }

        public ItemSetDto GetItemSetAndDetail(int id)
        {
            ItemSetDto ItemSets = (from itsm in Db.ItemSet_MT
                                   where itsm.ItemSetIDSys == id
                                   select new ItemSetDto
                                   {
                                       Active = itsm.Active,
                                       ItemSetCode = itsm.ItemSetCode,
                                       ItemSetDetail = GetItemSetDetail(id).ToList(),
                                       ItemSetIDSys = itsm.ItemSetIDSys,
                                       ItemSetName = itsm.ItemSetName,
                                       LineID = itsm.LineID,
                                       ProjectIDSys = itsm.ProjectIDSys,
                                       UserUpdate = itsm.UserUpdate
                                   }).FirstOrDefault();
            return ItemSets;
        }

        public IEnumerable<ItemSetDetailDto> GetItemSetDetail(int itemSetIDSys)
        {
            IEnumerable<ItemSetDetailDto> ItemSetDetail = (from itmsd in Db.ItemSetDetail
                                                           join itm in Db.Item_MT on itmsd.ItemIDSys equals itm.ItemIDSys
                                                           where itmsd.ItemSetIDSys == itemSetIDSys
                                                           select new ItemSetDetailDto
                                                           {
                                                               IDSys = itmsd.IDSys,
                                                               ItemCode = itm.ItemCode,
                                                               ItemIDSys = itmsd.ItemIDSys,
                                                               ItemName = itm.ItemName,
                                                               ItemSetIDSys = itmsd.ItemSetIDSys,
                                                               Qty = itmsd.Qty
                                                           }
                                                           ).ToList();
            return ItemSetDetail;
        }

        //public int CreateItemSet(ItemSet_MT ItemSet)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            repo.Insert(ItemSet);
        //            scope.Complete();
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
        //            throw ex;
        //        }

        //        return ItemSet.ItemSetIDSys;
        //    }
        //}

        public void UpdateItemSet(int id, ItemSet_MT ItemSet)
        {
            using (var scope = new TransactionScope())
            {

                ItemSet_MT existedItemSet = this.Get(id);
                existedItemSet.ItemSetCode = ItemSet.ItemSetCode;
                existedItemSet.ItemSetDetails = ItemSet.ItemSetDetails;
                existedItemSet.UpdateDate = DateTime.Now;
                existedItemSet.UserUpdate = ItemSet.UserUpdate;
                Db.SaveChanges();
            }
        }

        public void DeleteItemSet(int id, ItemSetDto ItemSet)
        {
            using (var scope = new TransactionScope())
            {
                var existedItemSet = this.GetByIDMT(id);
                existedItemSet.Active = 0;
                existedItemSet.UpdateDate = DateTime.Now;
                existedItemSet.UserUpdate = ItemSet.UserUpdate;
                Db.SaveChanges();
            }
        }

        public IEnumerable<ItemSetDto> Get()
        {
            return Get(50);
        }

        public IEnumerable<ItemSetDto> Get(int limit)
        {
            IEnumerable<ItemSetDto> ItemSets = (from itsm in Db.ItemSet_MT
                                                select new ItemSetDto
                                                {
                                                    Active = itsm.Active,
                                                    ItemSetCode = itsm.ItemSetCode,
                                                    ItemSetDetail = GetItemSetDetail(itsm.ItemSetIDSys).ToList(),
                                                    ItemSetIDSys = itsm.ItemSetIDSys,
                                                    ItemSetName = itsm.ItemSetName,
                                                    LineID = itsm.LineID,
                                                    ProjectIDSys = itsm.ProjectIDSys,
                                                    UserUpdate = itsm.UserUpdate
                                                }).ToList();
            return ItemSets;
        }




        public ItemSet_MT GetByID(object id)
        {
            throw new NotImplementedException();
        }

        public void Insert(ItemSet_MT entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(ItemSet_MT entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(ItemSet_MT entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemSet_MT> GetMany(Func<ItemSet_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ItemSet_MT> GetManyQueryable(Func<ItemSet_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public ItemSet_MT Get(Func<ItemSet_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(Func<ItemSet_MT, bool> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemSet_MT> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ItemSet_MT> GetWithInclude(Expression<Func<ItemSet_MT, bool>> predicate, params string[] include)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object primaryKey)
        {
            throw new NotImplementedException();
        }

        public ItemSet_MT GetSingle(Func<ItemSet_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public ItemSet_MT GetFirst(Func<ItemSet_MT, bool> predicate)
        {
            throw new NotImplementedException();
        }



























        //public int CreateItemSet(ItemSetDto ItemSet)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        ItemSet_MT item = new ItemSet_MT();
        //        item.ItemSetName = ItemSet.ItemSetName;
        //        item.ProjectIDSys = ItemSet.ProjectIDSys;
        //        item.LineID = ItemSet.LineID;
        //        item.ItemSetCode = proc.ProcGetNewID("IS").FirstOrDefault();
        //        item.CreatedDate = DateTime.Now;
        //        item.UpdateDate = DateTime.Now;
        //        item.UserUpdate = "1";


        //        try
        //        {
        //            repo.Insert(item);
        //            scope.Complete();
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
        //            throw ex;
        //        }
        //        return item.ItemSetIDSys;
        //    }
        //}

        //public int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        ItemSetDetail item = new ItemSetDetail();
        //        List<ItemSetDetail> itemLIst = new List<ItemSetDetail>();
        //        try
        //        {
        //            foreach (var c in temp)
        //            {
        //                item.Qty = c.Qty;
        //                item.ItemIDSys = c.ItemIDSys;
        //                item.ItemSetIDSys = id;
        //                itemLIst.Add(item);
        //            }
        //            Db.SaveChanges();
        //            //repo.Insert(itemLIst);
        //            scope.Complete();
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
        //            throw ex;
        //        }
        //        return item.ItemIDSys;
        //    }
        //}

        //public void HandleValidationException(DbEntityValidationException ex)
        //{
        //    foreach (var eve in ex.EntityValidationErrors)
        //    {
        //        foreach (var ve in eve.ValidationErrors)
        //        {
        //            throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
        //        }
        //    }
        //}

        //public ItemSetDto GetItemSet(int id)
        //{
        //    var item = repo.GetItemSetDto(id);
        //    var items = repo.GetItemSetDetailDto(id);
        //    item.ItemSetDetail = items;
        //    return item;
        //}

        //public bool DeleteItemSetDto(int id)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            repo.Delete(id);
        //            scope.Complete();
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
        //            throw ex;
        //        }
        //    }

        //    return true;
        //}
    }
}