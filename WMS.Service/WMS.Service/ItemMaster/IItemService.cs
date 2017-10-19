using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{
    public interface IItemService
    {
        IEnumerable<ItemDto> GetItems();
        ItemDto GetItem(int id, string[] tableNames);        
        int CreateItem(Item_MT Item);
        bool UpdateItem(int id, Item_MT Item);
        bool DeleteItem(int id);        
    }
}
