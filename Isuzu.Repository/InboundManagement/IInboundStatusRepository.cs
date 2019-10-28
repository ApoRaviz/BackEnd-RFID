using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;

namespace Isuzu.Repository.ItemManagement
{
    public interface IInboundStatusRepository : IRepository<InboundItemsStatus>
    {
        //Custom 
        //IEnumerable<InboundItemsHead> GetItemAll(int max);
        //InboundItemsHead GetItemBy(Func<InboundItemsHead, bool> where, bool isIncludeChild = false);
        //InboundItemsHead GetItemFirstBy(Func<InboundItemsHead, bool> where, bool isIncludeChild = false);
        //InboundItemsHead GetItemSingleBy(Func<InboundItemsHead, bool> where, bool isIncludeChild = false);
        //bool IsItemExistBy(Func<InboundItemsHead, bool> where);
        //IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);

    }
}
