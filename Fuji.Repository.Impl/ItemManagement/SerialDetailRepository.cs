using Fuji.Context;
using Fuji.Entity.ItemManagement;
using Fuji.Repository.ItemManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace Fuji.Repository.Impl.ItemManagement
{
    public class SerialDetailRepository : Repository<ImportSerialDetail>, ISerialDetailRepository
    {
        //private FujiDbContext Context { get; set; }
        private FujiDbContext Db { get; set; }
        private DbSet<ImportSerialDetail> DbSet { get; set; }

        public SerialDetailRepository(FujiDbContext context) : base(context)
        {
            Db = context;
            this.DbSet = context.Set<ImportSerialDetail>();
        }

        //#region Inherite Method

        //public virtual IEnumerable<ImportSerialDetail> Get()
        //{
        //    IQueryable<ImportSerialDetail> query = DbSet;
        //    return query.ToList();
        //}

        //public virtual ImportSerialDetail GetByID(object id)
        //{
        //    return DbSet.Find(id);
        //}

        //public virtual void Insert(ImportSerialDetail entity)
        //{
        //    DbSet.Add(entity);
        //}

        //public virtual void Delete(object id)
        //{
        //    ImportSerialDetail entityToDelete = DbSet.Find(id);
        //    Delete(entityToDelete);
        //}

        //public virtual void Delete(ImportSerialDetail entityToDelete)
        //{
        //    if (Context.Entry(entityToDelete).State == EntityState.Detached)
        //    {
        //        DbSet.Attach(entityToDelete);
        //    }
        //    DbSet.Remove(entityToDelete);
        //}

        //public virtual void Update(ImportSerialDetail entityToUpdate)
        //{
        //    DbSet.Attach(entityToUpdate);
        //    Context.Entry(entityToUpdate).State = EntityState.Modified;
        //}

        //public virtual IEnumerable<ImportSerialDetail> GetMany(Func<ImportSerialDetail, bool> where)
        //{
        //    return DbSet.Where(where).ToList();
        //}


        //public virtual IQueryable<ImportSerialDetail> GetManyQueryable(Func<ImportSerialDetail, bool> where)
        //{
        //    return DbSet.Where(where).AsQueryable();
        //}

        //public ImportSerialDetail Get(Func<ImportSerialDetail, Boolean> where)
        //{
        //    return DbSet.Where(where).FirstOrDefault<ImportSerialDetail>();
        //}

        //public void Delete(Func<ImportSerialDetail, Boolean> where)
        //{
        //    IQueryable<ImportSerialDetail> objects = DbSet.Where<ImportSerialDetail>(where).AsQueryable();
        //    foreach (ImportSerialDetail obj in objects)
        //        DbSet.Remove(obj);
        //}

        //public virtual IEnumerable<ImportSerialDetail> GetAll()
        //{
        //    return DbSet.ToList();
        //}

        //public IQueryable<ImportSerialDetail> GetWithInclude(System.Linq.Expressions.Expression<Func<ImportSerialDetail, bool>> predicate, params string[] include)
        //{
        //    IQueryable<ImportSerialDetail> query = this.DbSet;
        //    query = include.Aggregate(query, (current, inc) => current.Include(inc));
        //    return query.Where(predicate);
        //}

        //public bool Exists(object primaryKey)
        //{
        //    return DbSet.Find(primaryKey) != null;
        //}

        //public ImportSerialDetail GetSingle(Func<ImportSerialDetail, bool> predicate)
        //{
        //    return DbSet.Single<ImportSerialDetail>(predicate);
        //}

        //public ImportSerialDetail GetFirst(Func<ImportSerialDetail, bool> predicate)
        //{
        //    return DbSet.First<ImportSerialDetail>(predicate);
        //}
        //#endregion

        //****CUSTOM****//


        public IEnumerable<ImportSerialDetail> GetItemAll(int max = 0)
        {
            if (max > 0)
                DbSet.Take(max);
            return DbSet.Take(max);
        }
        public ImportSerialDetail GetItemBy(Func<ImportSerialDetail, bool> where)
        {
            return DbSet.Find(where);
        }
        public ImportSerialDetail GetItemFirstBy(Func<ImportSerialDetail, bool> where)
        {
            return DbSet.FirstOrDefault(where);
        }
        public ImportSerialDetail GetItemSingleBy(Func<ImportSerialDetail, bool> where)
        {
            return DbSet.SingleOrDefault(where);
        }
        public IEnumerable<ImportSerialDetail> GetItemsBy(Func<ImportSerialDetail, bool> where)
        {
            return DbSet.Where(where);
        }

        public IEnumerable<T> SqlQuery<T>(string sql,params object[] parameters)
        {
            return this.Context.Database.SqlQuery<T>(sql, parameters);
        }

        public void ExceuteSql(string sql)
        {
          this.Context.Database.ExecuteSqlCommand(sql);
        }

        public bool IsAnyItemBy(Func<ImportSerialDetail, bool> where)
        {
            return DbSet.Any(where);
        }
        public void InsertItem(ImportSerialDetail item,string userName)
        {
            item.CreateBy = userName;
            item.CreateAt = DateTime.Now;
            item.UpdateBy = userName;
            item.UpdateAt = DateTime.Now;

            DbSet.Add(item);
            Db.SaveChanges();
        }

        public void UpdateItem(ImportSerialDetail item, string userName)
        {
            item.UpdateBy = userName;
            item.UpdateAt = DateTime.Now;

            DbSet.Attach(item);
            Context.Entry(item).State = EntityState.Modified;
            Db.SaveChanges();
        }

        public void DeleteItems(Func<ImportSerialDetail, bool> predicate)
        {
           var items = DbSet.Where(predicate);
            if(items != null)
                DbSet.RemoveRange(items);
            Db.SaveChanges();
        }

    }
}
