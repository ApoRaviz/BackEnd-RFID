using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WIM.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> Get();
        TEntity Get(Func<TEntity, Boolean> where);
        IEnumerable<TEntity> GetAll();
        TEntity GetByID(object id);
        bool Exists(object id);        
        TEntity Insert(TEntity entity);
        TEntity Update(object entityToUpdate);
        void Delete(object id);
        void Delete(TEntity entityToDelete);

        // Other
        IEnumerable<TEntity> GetMany(Func<TEntity, bool> where);
        IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where);        
        void Delete(Func<TEntity, Boolean> where);        
        //IQueryable<TEntity> GetWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params string[] include);
        IQueryable<TEntity> GetWithInclude(Func<TEntity, bool> where, params string[] include);
        TEntity GetSingle(Func<TEntity, bool> predicate);
        TEntity GetFirst(Func<TEntity, bool> predicate);
    }
}
