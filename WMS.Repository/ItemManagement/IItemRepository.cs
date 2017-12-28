using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
