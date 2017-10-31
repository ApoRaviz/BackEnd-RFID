using Fuji.Entity.ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;

namespace Fuji.Repository.ItemManagement
{
    public interface ISerialRepository: IRepository<ImportSerialDetail>
    {
        //IEnumerable<ImportSerialDetail> Get();
        //ImportSerialDetail GetByID(object id);
        //void Insert(ImportSerialDetail entity);
        //void Delete(object id);
        //void Delete(ImportSerialDetail entityToDelete);
        //void Update(ImportSerialDetail entityToUpdate);
        //IEnumerable<ImportSerialDetail> GetMany(Func<ImportSerialDetail, bool> where);
        //IQueryable<ImportSerialDetail> GetManyQueryable(Func<ImportSerialDetail, bool> where);
        //ImportSerialDetail Get(Func<ImportSerialDetail, Boolean> where);
        //void Delete(Func<ImportSerialDetail, Boolean> where);
        //IEnumerable<ImportSerialDetail> GetAll();
        //IQueryable<ImportSerialDetail> GetWithInclude(System.Linq.Expressions.Expression<Func<ImportSerialDetail, bool>> predicate, params string[] include);
        //bool Exists(object primaryKey);
        //ImportSerialDetail GetSingle(Func<ImportSerialDetail, bool> predicate);
        //ImportSerialDetail GetFirst(Func<ImportSerialDetail, bool> predicate);


        //Custom 
        IEnumerable<ImportSerialDetail> GetItemAll(int max = 0);
        ImportSerialDetail GetItemBy(Func<ImportSerialDetail, bool> where);
        ImportSerialDetail GetItemFirstBy(Func<ImportSerialDetail, bool> where);
        ImportSerialDetail GetItemSingleBy(Func<ImportSerialDetail, bool> where);
        IEnumerable<ImportSerialDetail> GetItemsBy(Func<ImportSerialDetail, bool> where);
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
        void ExceuteSql(string sql);
        bool IsAnyItemBy(Func<ImportSerialDetail, bool> where);
        void InsertItem(ImportSerialDetail item);
        void DeleteItems(Func<ImportSerialDetail, bool> predicate);
    }
}
