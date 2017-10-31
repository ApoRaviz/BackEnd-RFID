using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface IItemSetService
    {
        IEnumerable<ItemSetDto> GetItemSets();
        ItemSetDto GetItemSet(int id);      
        int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp , string username);
        int CreateItemSet(ItemSet_MT ItemSet , string username);
        int CreateItemSet(ItemSetDto ItemSet , string username);
        bool UpdateItemSet(ItemSet_MT ItemSet , string username);
        bool DeleteItemSet(int id);
        bool DeleteItemSetDto(int id);
    }
}
