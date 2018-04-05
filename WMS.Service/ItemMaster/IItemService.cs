using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface IItemService : IService
    {
        IEnumerable<ItemDto> GetItems();
        ItemDto GetItem(int id, string[] tableNames);        
        int CreateItem(Item_MT Item );
        int CreateItemGift(ItemGiftDto item);
        ItemUnitMapping CreateItemUnit(ItemUnitMapping itemunit);
        bool UpdateItem(Item_MT Item );
        bool DeleteItem(int id);
        bool DeleteItemUnit(ItemUnitMapping item);
        IEnumerable<AutocompleteItemDto> AutocompleteItem(string term);
    }
}
