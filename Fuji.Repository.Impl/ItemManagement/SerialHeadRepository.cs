using Fuji.Context;
using Fuji.Entity.ItemManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Fuji.Common.ValueObject;
using Fuji.Repository.ItemManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace Fuji.Repository.Impl.ItemManagement
{
    public class SerialHeadRepository : Repository<ImportSerialHead>,ISerialHeadRepository
    {
        private FujiDbContext Db { get; set; }
        private DbSet<ImportSerialHead> DbSet { get; set; }
        private IIdentity Identity { get; set; }

        public SerialHeadRepository(FujiDbContext context,IIdentity identity) :base(context,identity)
        {
            DbSet = context.Set<ImportSerialHead>();
            Db = context;
            Identity = identity;
        }

        //#region Inherite Method

        //public virtual IEnumerable<ImportSerialHead> Get()
        //{
        //    IQueryable<ImportSerialHead> query = DbSet;
        //    return query.ToList();
        //}

        //public virtual ImportSerialHead GetByID(object id)
        //{
        //    return DbSet.Find(id);
        //}

        //public virtual void Insert(ImportSerialHead entity)
        //{
        //    DbSet.Add(entity);
        //    this.Context.SaveChanges();
        //}

        //public virtual void Delete(object id)
        //{
        //    ImportSerialHead entityToDelete = DbSet.Find(id);
        //    Delete(entityToDelete);
        //}

        //public virtual void Delete(ImportSerialHead entityToDelete)
        //{
        //    if (Context.Entry(entityToDelete).State == EntityState.Detached)
        //    {
        //        DbSet.Attach(entityToDelete);
        //    }
        //    DbSet.Remove(entityToDelete);
        //    this.Context.SaveChanges();
        //}

        //public virtual void Update(ImportSerialHead entityToUpdate)
        //{
        //    DbSet.Attach(entityToUpdate);
        //    Context.Entry(entityToUpdate).State = EntityState.Modified;
        //    this.Context.SaveChanges();
        //}

        //public virtual IEnumerable<ImportSerialHead> GetMany(Func<ImportSerialHead, bool> where)
        //{
        //    return DbSet.Where(where).ToList();
        //}


        //public virtual IQueryable<ImportSerialHead> GetManyQueryable(Func<ImportSerialHead, bool> where)
        //{
        //    return DbSet.Where(where).AsQueryable();
        //}

        //public ImportSerialHead Get(Func<ImportSerialHead, Boolean> where)
        //{
        //    return DbSet.Where(where).FirstOrDefault<ImportSerialHead>();
        //}

        //public void Delete(Func<ImportSerialHead, Boolean> where)
        //{
        //    IQueryable<ImportSerialHead> objects = DbSet.Where<ImportSerialHead>(where).AsQueryable();
        //    foreach (ImportSerialHead obj in objects)
        //        DbSet.Remove(obj);
        //    this.Context.SaveChanges();
        //}

        //public virtual IEnumerable<ImportSerialHead> GetAll()
        //{
        //    return DbSet.ToList();
        //}

        //public IQueryable<ImportSerialHead> GetWithInclude(System.Linq.Expressions.Expression<Func<ImportSerialHead, bool>> predicate, params string[] include)
        //{
        //    IQueryable<ImportSerialHead> query = this.DbSet;
        //    query = include.Aggregate(query, (current, inc) => current.Include(inc));
        //    return query.Where(predicate);
        //}

        //public bool Exists(object primaryKey)
        //{
        //    return DbSet.Find(primaryKey) != null;
        //}

        //public ImportSerialHead GetSingle(Func<ImportSerialHead, bool> predicate)
        //{
        //    return DbSet.Single<ImportSerialHead>(predicate);
        //}

        //public ImportSerialHead GetFirst(Func<ImportSerialHead, bool> predicate)
        //{
        //    return DbSet.First<ImportSerialHead>(predicate);
        //}

        //#endregion

        //****CUSTOM****//

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return this.Context.Database.SqlQuery<T>(sql, parameters);
        }
        public T SqlQuerySingle<T>(string sql, params object[] parameters)
        {
            return this.Context.Database.SqlQuery<T>(sql, parameters).SingleOrDefault();
        }

        public IEnumerable<ImportSerialHead> GetItemAll(int max = 0)
        {
            var items = DbSet.Where(w => !w.Equals('0') && !w.Equals(FujiStatus.DELETED.ToString()));
            if (max > 0)
                items.Take(max);
            return items;
        }

        public ImportSerialHead GetItemBy(Func<ImportSerialHead, bool> where, bool isIncludeChild = false)
        {
            var item = DbSet.Where(where).FirstOrDefault();
            if (isIncludeChild)
                Context.Entry(item).Collection(c => c.ImportSerialDetail).Load();
            return item;
        }
        public ImportSerialHead GetItemFirstBy(Func<ImportSerialHead, bool> where, bool isIncludeChild = false)
        {
            var item = DbSet.FirstOrDefault(where);
            if (isIncludeChild)
                Context.Entry(item).Collection(c => c.ImportSerialDetail).Load();
            return item;
        }
        public ImportSerialHead GetItemSingleBy(Func<ImportSerialHead, bool> where,bool isIncludeChild = false)
        {
            var item = DbSet.SingleOrDefault(where);
            if (isIncludeChild)
                Context.Entry(item).Collection(c => c.ImportSerialDetail).Load();
            return item;
        }
        public IEnumerable<ImportSerialHead> GetItemsBy(Func<ImportSerialHead, bool> where)
        {
            return DbSet.Where(where);
        }

        public void InsertItem(ImportSerialHead item, bool isIncludeChild = false)
        {
            var ret = DbSet.Add(item);
            Db.SaveChanges();  
        }

        public void UpdateItem(ImportSerialHead item, string username)
        {
            item.UpdateBy = username;
            item.UpdateAt = DateTime.Now;

            DbSet.Attach(item);
            Context.Entry(item).State = EntityState.Modified;
            Db.SaveChanges();
        }

        public ImportSerialHead IncludeChild(ImportSerialHead item)
        {
            this.Context.Entry(item).Collection(c => c.ImportSerialDetail).Load();
            return item;
        }
    }
}
