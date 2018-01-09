using System.Collections.Generic;
using WIM.Core.Repository;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;

namespace WMS.Repository
{
    public interface IItemRepository : IRepository<Item_MT>
    {
        Item_MT GetManyWithUnit(int id);
        IEnumerable<AutocompleteItemDto> AutocompleteItem(string term);
    }
}
