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

        //Custom 
        InboundItems GetItemBy(Func<InboundItems, bool> where);
        InboundItems GetItemFirstBy(Func<InboundItems, bool> where);
        InboundItems GetItemSingleBy(Func<InboundItems, bool> where);
        IEnumerable<InboundItems> GetItemsBy(Func<InboundItems, bool> where);
        bool IsItemExistBy(Func<InboundItems, bool> where);
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
    }
}
