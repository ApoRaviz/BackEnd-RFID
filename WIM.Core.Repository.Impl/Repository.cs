using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity;

namespace WIM.Core.Repository.Impl
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbContext Context;
        internal DbSet<TEntity> DbSet;
        internal IIdentity Identity;

        public Repository(DbContext context, IIdentity identity)
        {
            Context = context;
            this.DbSet = Context.Set<TEntity>();
            Identity = identity;
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

            public bool Exists(object id)
        {
            return DbSet.Find(id) != null;
        }       

        public TEntity Insert(TEntity entityToInsert)
        {
            Type typeEntityToInsert = entityToInsert.GetType(); 
            PropertyInfo[] properties = typeEntityToInsert.GetProperties();

            TEntity entityForInsert = (TEntity)Activator.CreateInstance(typeof(TEntity), new object[] { });

            Type typeEntityForUpdate = entityForInsert.GetType();
            foreach (PropertyInfo prop in properties)
            {                
                var value = prop.GetValue(entityToInsert, null);                
                if (!prop.PropertyType.IsGenericType)
                {
                    typeEntityForUpdate.GetProperty(prop.Name).SetValue(entityForInsert, value, null);
                }
            }
            
            entityForInsert.CreateBy = Identity.Name;
            entityForInsert.CreateAt = DateTime.Now;
            entityForInsert.UpdateBy = Identity.Name;
            entityForInsert.UpdateAt = DateTime.Now;
            entityForInsert.IsActive = true;

            DbSet.Add(entityForInsert);
            return entityForInsert;
        }                

        public TEntity Update(object entityToUpdate)
        {
            Type typeEntityToUpdate = entityToUpdate.GetType();
            PropertyInfo[] properties = typeEntityToUpdate.GetProperties();
            string namePropKey = GetPropertyNameOfKeyAttribute(properties);
            var id = typeEntityToUpdate.GetProperty(namePropKey).GetValue(entityToUpdate, null);
            TEntity entityForUpdate = GetByID(id);
            if (entityForUpdate == null)
            {
                throw new Exception("Data Not Found.");
            }
            Type typeEntityForUpdate = entityForUpdate.GetType();
            foreach (PropertyInfo prop in properties)
            {
                var value = prop.GetValue(entityToUpdate);
                if (typeEntityForUpdate.GetProperty(prop.Name) != null && !prop.PropertyType.IsGenericType)
                {
                    typeEntityForUpdate.GetProperty(prop.Name).SetValue(entityForUpdate, value, null);
                }
            }
            entityForUpdate.UpdateBy = Identity.Name;
            entityForUpdate.UpdateAt = DateTime.Now;
            return entityForUpdate;
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

        public IQueryable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate).AsQueryable();
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
