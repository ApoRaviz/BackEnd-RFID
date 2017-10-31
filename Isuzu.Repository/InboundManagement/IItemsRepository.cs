using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Repository.ItemManagement
{
    public interface IItemsRepository<InboundItems> where InboundItems : class
    {
        IEnumerable<InboundItems> Get();
        InboundItems GetByID(object id);
        void Insert(InboundItems entity);
        void Delete(object id);
        void Delete(InboundItems entityToDelete);
        void Update(InboundItems entityToUpdate);
        IEnumerable<InboundItems> GetMany(Func<InboundItems, bool> where);
        IQueryable<InboundItems> GetManyQueryable(Func<InboundItems, bool> where);
        InboundItems Get(Func<InboundItems, Boolean> where);
        void Delete(Func<InboundItems, Boolean> where);
        IEnumerable<InboundItems> GetAll();
        IQueryable<InboundItems> GetWithInclude(System.Linq.Expressions.Expression<Func<InboundItems, bool>> predicate, params string[] include);
        bool Exists(object primaryKey);
        InboundItems GetSingle(Func<InboundItems, bool> predicate);
        InboundItems GetFirst(Func<InboundItems, bool> predicate);


        //Custom 
        IEnumerable<InboundItems> SqlQuery(string sql,params object[] parameters);
        bool Exists(Func<InboundItems, Boolean> where);
    }
}
