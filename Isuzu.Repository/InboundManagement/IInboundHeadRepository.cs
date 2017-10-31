using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;

namespace Isuzu.Repository.ItemManagement
{
    public interface IInboundHeadRepository : IRepository<InboundItemsHead>
    {
        //IEnumerable<InboundItemsHead> Get();
        //InboundItemsHead GetByID(object id);
        //void Insert(InboundItemsHead entity);
        //void Delete(object id);
        //void Delete(InboundItemsHead entityToDelete);
        //void Update(InboundItemsHead entityToUpdate);
        //IEnumerable<InboundItemsHead> GetMany(Func<InboundItemsHead, bool> where);
        //IQueryable<InboundItemsHead> GetManyQueryable(Func<InboundItemsHead, bool> where);
        //InboundItemsHead Get(Func<InboundItemsHead, Boolean> where);
        //void Delete(Func<InboundItemsHead, Boolean> where);
        //IEnumerable<InboundItemsHead> GetAll();
        //IQueryable<InboundItemsHead> GetWithInclude(System.Linq.Expressions.Expression<Func<InboundItemsHead, bool>> predicate, params string[] include);
        //bool Exists(object primaryKey);
        //InboundItemsHead GetSingle(Func<InboundItemsHead, bool> predicate);
        //InboundItemsHead GetFirst(Func<InboundItemsHead, bool> predicate, bool isIncludeChild = false);


        //Custom 
        IEnumerable<InboundItemsHead> GetItemAll(int max);
        InboundItemsHead GetItemBy(Func<InboundItemsHead, bool> where, bool isIncludeChild = false);
        InboundItemsHead GetItemFirstBy(Func<InboundItemsHead, bool> where, bool isIncludeChild = false);
        InboundItemsHead GetItemSingleBy(Func<InboundItemsHead, bool> where, bool isIncludeChild = false);
        bool IsItemExistBy(Func<InboundItemsHead, bool> where);
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);

    }
}
