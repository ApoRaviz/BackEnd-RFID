using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Repository.ItemManagement
{
    public interface ISerialHeadRepository<ImportSerialHead> where ImportSerialHead:class
    {
        IEnumerable<ImportSerialHead> Get();
        ImportSerialHead GetByID(object id);
        ImportSerialHead Insert(ImportSerialHead entity, bool isIncludeChild = false);
        void Delete(object id);
        void Delete(ImportSerialHead entityToDelete);
        void Update(ImportSerialHead entityToUpdate);
        IEnumerable<ImportSerialHead> GetMany(Func<ImportSerialHead, bool> where);
        IQueryable<ImportSerialHead> GetManyQueryable(Func<ImportSerialHead, bool> where);
        ImportSerialHead Get(Func<ImportSerialHead, Boolean> where);
        void Delete(Func<ImportSerialHead, Boolean> where);
        IEnumerable<ImportSerialHead> GetAll();
        IQueryable<ImportSerialHead> GetWithInclude(System.Linq.Expressions.Expression<Func<ImportSerialHead, bool>> predicate, params string[] include);
        bool Exists(object primaryKey);
        ImportSerialHead GetSingle(Func<ImportSerialHead, bool> predicate);
        ImportSerialHead GetFirst(Func<ImportSerialHead, bool> predicate, bool isIncludeChild = false);


        //Custom 
        IEnumerable<ImportSerialHead> SqlQuery(string sql, params object[] parameters);
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
        T SqlQueryFirst<T>(string sql, params object[] parameters);
        bool Exists(Func<ImportSerialHead, Boolean> where);
    }
}
