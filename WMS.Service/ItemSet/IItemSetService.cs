using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface IItemSetService : IService
    {
        IEnumerable<ItemSetDto> GetItemSets();
        ItemSetDto GetItemSet(int id);
        IEnumerable<ItemSetDto> GetDto(int limit);
        int CreateItemsetDetail(int id, List<ItemSetDetailDto> temp);
        object CreateItemSet(ItemSet_MT ItemSet);
        int CreateItemSet(ItemSetDto ItemSet);
        bool UpdateItemSet(int id, ItemSet_MT ItemSet);
        int UpdateItemSet(ItemSet_MT ItemSet);
        bool DeleteItemSet(int id);
        /// <summary>
        /// delete item set detail in item set
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteItemSetDto(int id); 
    }
}
