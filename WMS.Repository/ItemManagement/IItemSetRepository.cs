
using System.Collections.Generic;
using System.Security.Principal;
using WIM.Core.Repository;
using WMS.Common;
using WMS.Entity.ItemManagement;

namespace WMS.Repository.ItemManagement 
{
    public interface IItemSetRepository : IRepository<ItemSet_MT>
    {
        //ItemSet_MT CreateItemSet(ItemSet_MT ItemSet, IIdentity identity);
        //ItemSet_MT UpdateItemSet(ItemSetDto ItemSet, IIdentity identity);
        ItemSetDto GetDtoByID(int id);
        ItemSetDto GetItemSetAndDetail(int id);
        IEnumerable<ItemSetDetailDto> GetDtoItemSetDetail(int itemSetIDSys);
        IEnumerable<ItemSetDto> GetDto();
        IEnumerable<ItemSetDto> GetDto(int limit);

    }


}
