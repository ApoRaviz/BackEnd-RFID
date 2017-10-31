using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using WIM.Core.Entity;

namespace WIM.Core.Repository.Impl
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbContext Context;
        internal DbSet<TEntity> DbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.DbSet = Context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return DbSet.Take(2000).ToList();
        }

        public TEntity Get(Func<TEntity, Boolean> where)
        {
            return DbSet.Where(where).FirstOrDefault<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public TEntity GetByID(object id)
        {
            return DbSet.Find(id);
        }       

            public bool Exists(object primaryKey)
        {
            return DbSet.Find(primaryKey) != null;
        }

        public TEntity Insert(TEntity entity, IIdentity identity)
        {            
            entity.CreateBy = identity.Name;
            entity.CreateAt = DateTime.Now;
            entity.UpdateBy = identity.Name;
            entity.UpdateAt = DateTime.Now;

            DbSet.Add(entity);
            return entity;
        }

        private string GetPropertyNameOfKeyAttribute(PropertyInfo[] properties)
        {
            foreach (PropertyInfo prop in properties)
            {
                KeyAttribute attr = prop.GetCustomAttribute<KeyAttribute>();
                if (attr != null)
                {
                    return prop.Name;
                }
            }
            throw new Exception("The Object Found KeyAttribute.");
        }

        public TEntity Update(object entityToUpdate, IIdentity identity)
        {
            Type typeEntityToUpdate = entityToUpdate.GetType(); 
            PropertyInfo[] properties = typeEntityToUpdate.GetProperties();
            string namePropKey = GetPropertyNameOfKeyAttribute(properties);         
            var id = typeEntityToUpdate.GetProperty(namePropKey).GetValue(entityToUpdate, null);
            TEntity entityForUpdated= GetByID(id);
            Type typeEntityForUpdate = entityForUpdated.GetType();
            foreach (PropertyInfo prop in properties)
            {                
                var value = prop.GetValue(entityToUpdate);                
                if (typeEntityForUpdate.GetProperty(prop.Name) != null)
                {
                    typeEntityForUpdate.GetProperty(prop.Name).SetValue(entityForUpdated, value, null);
                }
            }
            entityForUpdated.UpdateBy = identity.Name;
            entityForUpdated.UpdateAt = DateTime.Now;
            return entityForUpdated;
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

       
        // Other
        public IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return DbSet.Where(where).ToList();
        }

        public IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            return DbSet.Where(where).AsQueryable();
        }
      
        public void Delete(Func<TEntity, Boolean> where)
        {
            IQueryable<TEntity> objects = DbSet.Where<TEntity>(where).AsQueryable();
            foreach (TEntity obj in objects)
                DbSet.Remove(obj);
        }

        public IQueryable<TEntity> GetWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        

        public TEntity GetSingle(Func<TEntity, bool> predicate)
        {
            return DbSet.Single<TEntity>(predicate);
        }

        public TEntity GetFirst(Func<TEntity, bool> predicate)
        {
            return DbSet.First<TEntity>(predicate);
        }       

    }
}
