using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;

namespace WMS.Repository.ItemManagement
{
    public interface IItemRepository
    {
        IEnumerable<ItemDto> GetItems();
    }
}
