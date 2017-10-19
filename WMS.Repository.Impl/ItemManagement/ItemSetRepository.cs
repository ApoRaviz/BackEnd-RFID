using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.ItemManagement;

namespace WMS.Repository.Impl
{
    public class ItemSetRepository : IGenericRepository<ItemSet_MT>
    {
        private WMSDbContext Db { get; set; }

        public ItemSetRepository()
        {
            Db = new WMSDbContext();
        }

        public IEnumerable<ItemSet_MT> Get()
        {
            IEnumerable<ItemSet_MT> ItemSets = (from i in Db.ItemSet_MT
                                                where i.Active == 1
                                                select i).ToList();
            return ItemSets;
        }

        public ItemSet_MT GetByID(object id)
        {
            var query = (from i in Db.ItemSet_MT
                         where i.ItemSetIDSys== (int)id
                         select i).SingleOrDefault();
            return query;
        }

        public ItemSetDto GetItemSetDto(int id)
        {
            var query = (from i in Db.ItemSet_MT
                         where i.ItemSetIDSys == id
                         select i);
            ItemSetDto item = query.Select(b => new ItemSetDto()
            {
                ItemSetCode = b.ItemSetCode,
                ItemSetIDSys = b.ItemSetIDSys,
                ItemSetName = b.ItemSetName,
                LineID = b.LineID,
                ProjectIDSys = b.ProjectIDSys
            }).SingleOrDefault();

            return item;
        }

        public List<ItemSetDetailDto> GetItemSetDetailDto(int id)
        {
            var query2 = (from row in Db.ItemSetDetail
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
            return items;
        }

        public void Insert(ItemSet_MT entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.UserUpdate = "1";
            entity.Active = 1;
            Db.ItemSet_MT.Add(entity);
            Db.SaveChanges();
        }

        public void Insert(ItemSetDetail item)
        {
            Db.ItemSetDetail.Add(item);
            Db.SaveChanges();
        }

        /// <summary>
        /// Delete Itemset Details
        /// </summary>
        /// <param name="id"></param>
        public void Delete(object id)
        {
            var query2 = from row in Db.ItemSetDetail
                         where row.ItemSetIDSys== (int)id
                         select row;
            Db.ItemSetDetail.RemoveRange(query2);
            Db.SaveChanges();
        }
        
        /// <summary>
        /// Delete ItemSet
        /// </summary>
        /// <param name="entityToDelete"></param>
        public void Delete(ItemSet_MT entityToDelete)
        {
            var existedItemSet = (from c in Db.ItemSet_MT
                                  where c.ItemSetIDSys.Equals(entityToDelete.ItemSetIDSys)
                                  select c).SingleOrDefault();
            existedItemSet.Active = 0;
            existedItemSet.UpdateDate = DateTime.Now;
            existedItemSet.UserUpdate = "1";
            Db.SaveChanges();
        }

        public void Update(ItemSet_MT entityToUpdate)
        {
            var existedItemSet = (from c in Db.ItemSet_MT
                                  where c.ItemSetIDSys.Equals(entityToUpdate.ItemSetIDSys)
                                  select c).SingleOrDefault();
            existedItemSet.ProjectIDSys = entityToUpdate.ProjectIDSys;
            existedItemSet.ItemSetCode = entityToUpdate.ItemSetCode;
            existedItemSet.UpdateDate = DateTime.Now;
            existedItemSet.UserUpdate = "1";
            Db.SaveChanges();
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
    }
}
