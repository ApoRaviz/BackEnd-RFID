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
    public interface IItemSetRepository : IRepository<ItemSet_MT>
    {
        ItemSetDto GetItemSetAndDetail(int id);
        ItemSetDto GetDtoByID(int id);
        IEnumerable<ItemSetDetailDto> GetDtoItemSetDetail(int itemSetIDSys);
        IEnumerable<ItemSetDto> GetDto();
    }
}
