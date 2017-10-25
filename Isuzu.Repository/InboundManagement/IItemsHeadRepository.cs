using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Repository.ItemManagement
{
    public interface IItemsHeadRepository<InboundItemsHead> where InboundItemsHead : class
    {
        IEnumerable<InboundItemsHead> Get();
        InboundItemsHead GetByID(object id);
        void Insert(InboundItemsHead entity);
        void Delete(object id);
        void Delete(InboundItemsHead entityToDelete);
        void Update(InboundItemsHead entityToUpdate);
        IEnumerable<InboundItemsHead> GetMany(Func<InboundItemsHead, bool> where);
        IQueryable<InboundItemsHead> GetManyQueryable(Func<InboundItemsHead, bool> where);
        InboundItemsHead Get(Func<InboundItemsHead, Boolean> where);
        void Delete(Func<InboundItemsHead, Boolean> where);
        IEnumerable<InboundItemsHead> GetAll();
        IQueryable<InboundItemsHead> GetWithInclude(System.Linq.Expressions.Expression<Func<InboundItemsHead, bool>> predicate, params string[] include);
        bool Exists(object primaryKey);
        InboundItemsHead GetSingle(Func<InboundItemsHead, bool> predicate);
        InboundItemsHead GetFirst(Func<InboundItemsHead, bool> predicate, bool isIncludeChild = false);


        //Custom 
        IEnumerable<InboundItemsHead> SqlQuery(string sql,params object[] parameters);
        bool Exists(Func<InboundItemsHead, bool> where);

    }
}
