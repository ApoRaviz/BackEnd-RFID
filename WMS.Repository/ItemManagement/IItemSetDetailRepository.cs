using System.Collections.Generic;
using System.Security.Principal;
using WIM.Core.Repository;
using WMS.Entity.ItemManagement;

namespace WMS.Repository.ItemManagement
{
    public interface IItemSetDetailRepository : IRepository<ItemSetDetail>
    {
        IEnumerable<ItemSetDetail> CreateItemSetDetail(IEnumerable<ItemSetDetail> ItemSetDetail, IIdentity identity);
        IEnumerable<ItemSetDetail> UpdateItemSetDetail(IEnumerable<ItemSetDetail> ItemSetDetail, IIdentity identity);
    }
}
