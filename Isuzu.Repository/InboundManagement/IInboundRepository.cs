using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;

namespace Isuzu.Repository.ItemManagement
{
    public interface IInboundRepository : IRepository<InboundItems>
    {
        //IEnumerable<InboundItems> Get();
        //InboundItems GetByID(object id);
        //void Insert(InboundItems entity);
        //void Delete(object id);
        //void Delete(InboundItems entityToDelete);
        //void Update(InboundItems entityToUpdate);
        //IEnumerable<InboundItems> GetMany(Func<InboundItems, bool> where);
        //IQueryable<InboundItems> GetManyQueryable(Func<InboundItems, bool> where);
        //InboundItems Get(Func<InboundItems, Boolean> where);
        //void Delete(Func<InboundItems, Boolean> where);
        //IEnumerable<InboundItems> GetAll();
        //IQueryable<InboundItems> GetWithInclude(System.Linq.Expressions.Expression<Func<InboundItems, bool>> predicate, params string[] include);
        //bool Exists(object primaryKey);
        //InboundItems GetSingle(Func<InboundItems, bool> predicate);
        //InboundItems GetFirst(Func<InboundItems, bool> predicate);


        //Custom 
        InboundItems GetItemBy(Func<InboundItems, bool> where);
        InboundItems GetItemFirstBy(Func<InboundItems, bool> where);
        InboundItems GetItemSingleBy(Func<InboundItems, bool> where);
        IEnumerable<InboundItems> GetItemsBy(Func<InboundItems, bool> where);
        bool IsItemExistBy(Func<InboundItems, bool> where);
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
    }
}
