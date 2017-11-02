using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface IItemSetService : IService
    {
        IEnumerable<ItemSetDto> GetItemSets();
        ItemSetDto GetItemSet(int id);      
        int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp);
        int CreateItemSet(ItemSet_MT ItemSet);
        int CreateItemSet(ItemSetDto ItemSet);
        bool UpdateItemSet(int id, ItemSet_MT ItemSet);
        bool DeleteItemSet(int id);
    }
}
