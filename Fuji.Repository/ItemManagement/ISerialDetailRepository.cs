using Fuji.Entity.ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;

namespace Fuji.Repository.ItemManagement
{
    public interface ISerialDetailRepository: IRepository<ImportSerialDetail>
    {
        //Custom 
        IEnumerable<ImportSerialDetail> GetItemAll(int max = 0);
        ImportSerialDetail GetItemBy(Func<ImportSerialDetail, bool> where);
        ImportSerialDetail GetItemFirstBy(Func<ImportSerialDetail, bool> where);
        ImportSerialDetail GetItemSingleBy(Func<ImportSerialDetail, bool> where);
        IEnumerable<ImportSerialDetail> GetItemsBy(Func<ImportSerialDetail, bool> where);
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
        void ExceuteSql(string sql);
        bool IsAnyItemBy(Func<ImportSerialDetail, bool> where);
        bool IsSerialsRemainInStock(List<string> itemGroups);
    }
}
