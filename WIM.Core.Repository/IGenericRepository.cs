using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace WIM.Core.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        IEnumerable<TEntity> GetMany(Func<TEntity, bool> where);
        IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where);
        TEntity Get(Func<TEntity, Boolean> where);
        void Delete(Func<TEntity, Boolean> where);
        IEnumerable<TEntity> GetAll();
        IQueryable<TEntity> GetWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params string[] include);
        bool Exists(object primaryKey);
        TEntity GetSingle(Func<TEntity, bool> predicate);
        TEntity GetFirst(Func<TEntity, bool> predicate);
    }

    public abstract class AGenericRepository<TEntity> where TEntity : class
    {
        public abstract IEnumerable<TEntity> Get();
        
        public abstract TEntity GetByID(object id);


        public TEntity GetCustomByID(object id)
        {
            return null;
        }
    }
}
