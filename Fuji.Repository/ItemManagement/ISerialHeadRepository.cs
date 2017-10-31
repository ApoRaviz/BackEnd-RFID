using Fuji.Entity.ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;

namespace Fuji.Repository.ItemManagement
{
    public interface ISerialHeadRepository : IRepository<ImportSerialHead>
    {
        //IEnumerable<ImportSerialHead> Get();
        //ImportSerialHead GetByID(object id);
        //ImportSerialHead Insert(ImportSerialHead entity, bool isIncludeChild = false);
        //void Delete(object id);
        //void Delete(ImportSerialHead entityToDelete);
        //void Update(ImportSerialHead entityToUpdate);
        //IEnumerable<ImportSerialHead> GetMany(Func<ImportSerialHead, bool> where);
        //IQueryable<ImportSerialHead> GetManyQueryable(Func<ImportSerialHead, bool> where);
        //ImportSerialHead Get(Func<ImportSerialHead, Boolean> where);
        //void Delete(Func<ImportSerialHead, Boolean> where);
        //IEnumerable<ImportSerialHead> GetAll();
        //IQueryable<ImportSerialHead> GetWithInclude(System.Linq.Expressions.Expression<Func<ImportSerialHead, bool>> predicate, params string[] include);
        //bool Exists(object primaryKey);
        //ImportSerialHead GetSingle(Func<ImportSerialHead, bool> predicate);
        //ImportSerialHead GetFirst(Func<ImportSerialHead, bool> predicate, bool isIncludeChild = false);


        //Custom 
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
        T SqlQuerySingle<T>(string sql, params object[] parameters);
        IEnumerable<ImportSerialHead> GetItemAll(int max = 0);

        ImportSerialHead GetItemBy(Func<ImportSerialHead, bool> where, bool isIncludeChild = false);
        ImportSerialHead GetItemFirstBy(Func<ImportSerialHead, bool> where, bool isIncludeChild = false);
        ImportSerialHead GetItemSingleBy(Func<ImportSerialHead, bool> where, bool isIncludeChild = false);
        IEnumerable<ImportSerialHead> GetItemsBy(Func<ImportSerialHead, bool> where);
        void InsertItem(ImportSerialHead item, bool isIncludeChild = false);
        ImportSerialHead IncludeChild(ImportSerialHead item);
    }
}
