using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Repository.ItemManagement
{
    public interface ISerialRepository<ImportSerialDetail> where ImportSerialDetail :class
    {
        IEnumerable<ImportSerialDetail> Get();
        ImportSerialDetail GetByID(object id);
        void Insert(ImportSerialDetail entity);
        void Delete(object id);
        void Delete(ImportSerialDetail entityToDelete);
        void Update(ImportSerialDetail entityToUpdate);
        IEnumerable<ImportSerialDetail> GetMany(Func<ImportSerialDetail, bool> where);
        IQueryable<ImportSerialDetail> GetManyQueryable(Func<ImportSerialDetail, bool> where);
        ImportSerialDetail Get(Func<ImportSerialDetail, Boolean> where);
        void Delete(Func<ImportSerialDetail, Boolean> where);
        IEnumerable<ImportSerialDetail> GetAll();
        IQueryable<ImportSerialDetail> GetWithInclude(System.Linq.Expressions.Expression<Func<ImportSerialDetail, bool>> predicate, params string[] include);
        bool Exists(object primaryKey);
        ImportSerialDetail GetSingle(Func<ImportSerialDetail, bool> predicate);
        ImportSerialDetail GetFirst(Func<ImportSerialDetail, bool> predicate);


        //Custom 
        IEnumerable<ImportSerialDetail> SqlQuery(string sql, params object[] parameters);
        bool Exists(Func<ImportSerialDetail, Boolean> where);
    }
}
