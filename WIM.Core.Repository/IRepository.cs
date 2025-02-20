﻿using System;
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
        IIdentity Identity { get; }

        IEnumerable<TEntity> Get();
        Task<IEnumerable<TEntity>> GetAsync();
        TEntity Get(Func<TEntity, Boolean> where);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        TEntity GetByID(object id);
        TEntity GetByID(params object[] id);
        TEntity GetByID(object id, bool isTryValidationNotNullException);
        TEntity GetByID(bool isTryValidationNotNullException, params object[] id);
        Task<TEntity> GetByIDAsync(params object[] id);
        Task<TEntity> GetByIDAsync(object id);
        bool Exists(object id);
        bool Exists(Func<TEntity, Boolean> where);
        TEntity Save(TEntity entity);
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entityToUpdate);
        void Delete(object id);
        void Delete(TEntity entityToDelete);

        // Other
        IEnumerable<TEntity> GetMany(Func<TEntity, bool> where);
        IEnumerable<TEntity> GetMany(Func<TEntity, bool> where, bool isTryValidationNotNullException);
        IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where);        
        void Delete(Func<TEntity, Boolean> where);        
        //IQueryable<TEntity> GetWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params string[] include);
        IQueryable<TEntity> GetWithInclude(Func<TEntity, bool> where, params string[] include);
        TEntity GetSingle(Func<TEntity, bool> predicate);
        TEntity GetFirst(Func<TEntity, bool> predicate);

        string GetValidation(List<string> tableName);
    }
}
