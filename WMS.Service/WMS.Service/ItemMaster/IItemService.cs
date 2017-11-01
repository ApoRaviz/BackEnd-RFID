using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface IItemService
    {
        IEnumerable<ItemDto> GetItems();
        ItemDto GetItem(int id, string[] tableNames);        
        int CreateItem(Item_MT Item );
        bool UpdateItem(Item_MT Item );
        bool DeleteItem(int id);        
    }
}
