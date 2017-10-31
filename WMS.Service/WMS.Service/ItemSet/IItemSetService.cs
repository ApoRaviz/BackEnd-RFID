using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface IItemSetService
    {
        IEnumerable<ItemSetDto> GetDto(int limit);
        ItemSetDto GetDtoByID(int id);
        ItemSetDto CreateItemSet(ItemSet_MT ItemSet);
        bool UpdateItemSet(ItemSetDto ItemSet);
        bool DeleteItemSet(int id);
    }
}
