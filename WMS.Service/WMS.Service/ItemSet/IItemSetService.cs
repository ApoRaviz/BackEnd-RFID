using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{
    public interface IItemSetService
    {
        IEnumerable<ItemSetDto> GetItemSets();
        ItemSetDto GetItemSet(int id);      
        int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp);
        int CreateItemSet(ItemSet_MT ItemSet);
        int CreateItemSet(ItemSetDto ItemSet);
        bool UpdateItemSet(int id, ItemSet_MT ItemSet);
        bool DeleteItemSet(int id);
        bool DeleteItemSetDto(int id);
    }
}
