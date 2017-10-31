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
        IEnumerable<ItemSetDto> GetDto();
        ItemSetDto GetDtoByID(int id,IIdentity UserIden);
        ItemSetDto CreateItemSet(ItemSet_MT ItemSet, IIdentity identity);
        ItemSetDto UpdateItemSet(ItemSetDto ItemSet, IIdentity identity);
        bool DeleteItemSet(int id,IIdentity UserIden);
    }
}
