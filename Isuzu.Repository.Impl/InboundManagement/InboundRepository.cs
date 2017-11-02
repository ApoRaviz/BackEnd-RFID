using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using Isuzu.Context;
using Isuzu.Repository.ItemManagement;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace Isuzu.Repository.Impl
{
    public class InboundRepository : Repository<InboundItems>, IInboundRepository
    {

        private IsuzuDataContext Db { get; set; }
        private DbSet<InboundItems> DbSet;
        private IIdentity Identity;

        public InboundRepository(IsuzuDataContext context,IIdentity identity) :base(context,identity)
        {
           Db = context;
           this.DbSet = context.Set<InboundItems>();
           Identity = identity;
        }

        //#region InheritMethod

        //public virtual IEnumerable<InboundItems> Get()
        //{
        //    IQueryable<InboundItems> query = DbSet;
        //    return query.ToList();
        //}

        //public virtual InboundItems GetByID(object id)
        //{
        //    return DbSet.Find(id);
        //}

        //public virtual void Insert(InboundItems entity)
        //{
        //    DbSet.Add(entity);
        //    this.Context.SaveChanges();
        //}

        //public virtual void Delete(object id)
        //{
        //    InboundItems entityToDelete = DbSet.Find(id);
        //    Delete(entityToDelete);
        //}

        //public virtual void Delete(InboundItems entityToDelete)
        //{
        //    if (Context.Entry(entityToDelete).State == EntityState.Detached)
        //    {
        //        DbSet.Attach(entityToDelete);
        //    }
        //    DbSet.Remove(entityToDelete);
        //    this.Context.SaveChanges();
        //}

        //public virtual void Update(InboundItems entityToUpdate)
        //{
        //    DbSet.Attach(entityToUpdate);
        //    Context.Entry(entityToUpdate).State = EntityState.Modified;
        //    this.Context.SaveChanges();
        //}

        //public virtual IEnumerable<InboundItems> GetMany(Func<InboundItems, bool> where)
        //{
        //    return DbSet.Where(where).ToList();
        //}


        //public virtual IQueryable<InboundItems> GetManyQueryable(Func<InboundItems, bool> where)
        //{
        //    return DbSet.Where(where).AsQueryable();
        //}

        //public InboundItems Get(Func<InboundItems, Boolean> where)
        //{
        //    return DbSet.Where(where).FirstOrDefault<InboundItems>();
        //}

        //public void Delete(Func<InboundItems, Boolean> where)
        //{
        //    IQueryable<InboundItems> objects = DbSet.Where<InboundItems>(where).AsQueryable();
        //    foreach (InboundItems obj in objects)
        //        DbSet.Remove(obj);
        //    this.Context.SaveChanges();
        //}

        //public virtual IEnumerable<InboundItems> GetAll()
        //{
        //    return DbSet.ToList();
        //}

        //public IQueryable<InboundItems> GetWithInclude(System.Linq.Expressions.Expression<Func<InboundItems, bool>> predicate, params string[] include)
        //{
        //    IQueryable<InboundItems> query = this.DbSet;
        //    query = include.Aggregate(query, (current, inc) => current.Include(inc));
        //    return query.Where(predicate);
        //}

        //public bool Exists(object primaryKey)
        //{
        //    return DbSet.Find(primaryKey) != null;
        //}

        //public InboundItems GetSingle(Func<InboundItems, bool> predicate)
        //{
        //    return DbSet.Single<InboundItems>(predicate);
        //}

        //public InboundItems GetFirst(Func<InboundItems, bool> predicate)
        //{
        //    return DbSet.First<InboundItems>(predicate);
        //}
        //#endregion

        //****CUSTOM****//

        public InboundItems GetItemBy(Func<InboundItems, bool> where)
        {
            return DbSet.Find(where);
        }
        public InboundItems GetItemFirstBy(Func<InboundItems, bool> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }
        public InboundItems GetItemSingleBy(Func<InboundItems, bool> where)
        {
            return DbSet.Where(where).SingleOrDefault();
        }

        public IEnumerable<InboundItems> GetItemsBy(Func<InboundItems, bool> where)
        {
            return DbSet.Where(where);
        }

        public bool IsItemExistBy(Func<InboundItems, bool> where)
        {
            return DbSet.Find(where) != null;
        }

        public IEnumerable<T> SqlQuery<T>(string sql,params object[] parameters)
        {
            return Context.Database.SqlQuery<T>(sql, parameters);
           }

        public void InsertItem(InboundItems entity, string username)
        {
            entity.CreateBy = username;
            entity.CreateAt = DateTime.Now;
            entity.UpdateBy = username;
            entity.UpdateAt = DateTime.Now;

            DbSet.Add(entity);
            Db.SaveChanges();
        }

        public void UpdateItem(InboundItems entityToUpdate, string username)
        {
            entityToUpdate.UpdateBy = username;
            entityToUpdate.UpdateAt = DateTime.Now;

            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
            Db.SaveChanges();
        }

    }
}
