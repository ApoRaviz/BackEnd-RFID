using Isuzu.Context;
using Isuzu.Entity;
using Isuzu.Repository;
using Isuzu.Repository.ItemManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace Isuzu.Repository.Impl
{
    public class InboundHeadRepository : Repository<InboundItemsHead>,IInboundHeadRepository
    {
        private IsuzuDataContext Db { get; set; }
        private DbSet<InboundItemsHead> DbSet;

        public InboundHeadRepository(IsuzuDataContext context) :base(context)
        {
            Db = context;
            this.DbSet = context.Set<InboundItemsHead>();
        }

        //#region InheritMethod

        //public virtual IEnumerable<InboundItemsHead> Get()
        //{
        //    IQueryable<InboundItemsHead> query = DbSet;
        //    return query.ToList();
        //}

        //public virtual InboundItemsHead GetByID(object id)
        //{
        //    return DbSet.Find(id);
        //}

        //public virtual void Insert(InboundItemsHead entity)
        //{
        //    DbSet.Add(entity);
        //    this.Context.SaveChanges();
        //}

        //public virtual void Delete(object id)
        //{
        //    InboundItemsHead entityToDelete = DbSet.Find(id);
        //    Delete(entityToDelete);
        //}

        //public virtual void Delete(InboundItemsHead entityToDelete)
        //{
        //    if (Context.Entry(entityToDelete).State == EntityState.Detached)
        //    {
        //        DbSet.Attach(entityToDelete);
        //    }
        //    DbSet.Remove(entityToDelete);
        //    this.Context.SaveChanges();
        //}

        //public virtual void Update(InboundItemsHead entityToUpdate)
        //{
        //    DbSet.Attach(entityToUpdate);
        //    Context.Entry(entityToUpdate).State = EntityState.Modified;
        //    this.Context.SaveChanges();
        //}

        //public virtual IEnumerable<InboundItemsHead> GetMany(Func<InboundItemsHead, bool> where)
        //{
        //    return DbSet.Where(where).ToList();
        //}


        //public virtual IQueryable<InboundItemsHead> GetManyQueryable(Func<InboundItemsHead, bool> where)
        //{
        //    return DbSet.Where(where).AsQueryable();
        //}

        //public InboundItemsHead Get(Func<InboundItemsHead, Boolean> where)
        //{
        //    return DbSet.Where(where).FirstOrDefault<InboundItemsHead>();
        //}

        //public void Delete(Func<InboundItemsHead, Boolean> where)
        //{
        //    IQueryable<InboundItemsHead> objects = DbSet.Where<InboundItemsHead>(where).AsQueryable();
        //    foreach (InboundItemsHead obj in objects)
        //        DbSet.Remove(obj);
        //    this.Context.SaveChanges();
        //}

        //public virtual IEnumerable<InboundItemsHead> GetAll()
        //{
        //    return DbSet.ToList();
        //}

        //public IQueryable<InboundItemsHead> GetWithInclude(System.Linq.Expressions.Expression<Func<InboundItemsHead, bool>> predicate, params string[] include)
        //{
        //    IQueryable<InboundItemsHead> query = this.DbSet;
        //    query = include.Aggregate(query, (current, inc) => current.Include(inc));
        //    return query.Where(predicate);
        //}

        //public bool Exists(object primaryKey)
        //{
        //    return DbSet.Find(primaryKey) != null;
        //}

        //public InboundItemsHead GetSingle(Func<InboundItemsHead, bool> predicate)
        //{
        //    return DbSet.Single<InboundItemsHead>(predicate);
        //}

        //public InboundItemsHead GetFirst(Func<InboundItemsHead, bool> predicate)
        //{
        //    return DbSet.First<InboundItemsHead>(predicate);
        //}
        //#endregion

        //****CUSTOM****//


        public IEnumerable<InboundItemsHead> GetItemAll(int max)
        {
            if (max > 0)
                return DbSet.Take(max);
            return DbSet;
        }
        public InboundItemsHead GetItemBy(Func<InboundItemsHead, bool> where, bool isIncludeChild = false)
        {
            var item = DbSet.Where(where).FirstOrDefault();
            if (isIncludeChild)
                Context.Entry(item).Collection(c => c.InboundItems).Load();
            return item;
        }
        public InboundItemsHead GetItemFirstBy(Func<InboundItemsHead, bool> where, bool isIncludeChild = false)
        {
            var item = DbSet.Where(where).FirstOrDefault();
            if (isIncludeChild)
                Context.Entry(item).Collection(c => c.InboundItems).Load();
            return item;
        }
        public InboundItemsHead GetItemSingleBy(Func<InboundItemsHead, bool> where, bool isIncludeChild = false)
        {
            return DbSet.Where(where).SingleOrDefault();
        }

        public bool IsItemExistBy(Func<InboundItemsHead, bool> where)
        {
            return DbSet.Where(where).FirstOrDefault() != null;
        }

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return Context.Database.SqlQuery<T>(sql, parameters);
        }

        public void InsertItem(InboundItemsHead entity, string username)
        {
            entity.CreateBy = username;
            entity.CreateAt = DateTime.Now;
            entity.UpdateBy = username;
            entity.UpdateAt = DateTime.Now;

            DbSet.Add(entity);
            Db.SaveChanges();
        }


        public void UpdateItem(InboundItemsHead entityToUpdate, string username)
        {
            entityToUpdate.UpdateBy = username;
            entityToUpdate.UpdateAt = DateTime.Now;

            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
            Db.SaveChanges();
        }

    }
}
