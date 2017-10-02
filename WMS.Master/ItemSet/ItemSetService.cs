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
    public class ItemSetService : IItemSetService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<ItemSet_MT> repo;
        private GenericRepository<ItemSetDetail> repo2;

        public ItemSetService()
        {
            repo = new GenericRepository<ItemSet_MT>(db);
            repo2 = new GenericRepository<ItemSetDetail>(db);
        }

        public IEnumerable<ItemSetDto> GetItemSets()
        {
            IEnumerable<ItemSet_MT> ItemSets = (from i in db.ItemSet_MT
                                          where i.Active == 1
                                          select i).ToList();

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
                ItemSet.CreatedDate = DateTime.Now;
                ItemSet.UpdateDate = DateTime.Now;
                ItemSet.UserUpdate = "1";

                repo.Insert(ItemSet);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
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
                repo.Update(existedItemSet);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
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
                db.SaveChanges();
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
                item.ItemSetCode = db.ProcGetNewID("IS").FirstOrDefault();
                item.CreatedDate = DateTime.Now;
                item.UpdateDate = DateTime.Now;
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
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return item.ItemSetIDSys;
            }
        }

        public int CreateItemsetDetail(int id, ItemSetDetailDto temp)
        {
            using (var scope = new TransactionScope())
            {
                ItemSetDetail item = new ItemSetDetail();
                item.Qty = temp.Qty;
                item.ItemIDSys = temp.ItemIDSys;
                item.ItemSetIDSys = id;
                repo2.Insert(item);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }

                scope.Complete();
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

            var query = (from i in db.ItemSet_MT
                         where i.ItemSetIDSys == id 
                         select i);
            ItemSetDto item = query.Select(b => new ItemSetDto() {
                ItemSetCode = b.ItemSetCode,
                ItemSetIDSys = b.ItemSetIDSys,
                ItemSetName = b.ItemSetName,
                LineID = b.LineID,
                ProjectIDSys = b.ProjectIDSys
            }).SingleOrDefault();

            var query2 = (from row in db.ItemSetDetails
                         where row.ItemSetIDSys == id
                         select row);
            List<ItemSetDetailDto> items = query2.Include(a => a.Item_MT).Select(b => new ItemSetDetailDto()
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

        public bool DeleteItemSetDto(int id)
        {
            var query2 = from row in db.ItemSetDetails
                         where row.ItemSetIDSys == id
                         select row;
            db.ItemSetDetails.RemoveRange(query2);
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                HandleValidationException(e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                throw ex;
            }

            return true;
        }
    }
}
