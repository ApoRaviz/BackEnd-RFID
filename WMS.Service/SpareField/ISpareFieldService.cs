using System.Collections.Generic;
using WIM.Core.Service;
using WMS.Entity.SpareField;

namespace WMS.Service
{
    public interface ISpareFieldService : IService
    {
        IEnumerable<SpareField> GetSpareField();
        SpareField GetSpareFieldBySpfIDSys(int id);
        IEnumerable<SpareField> GetSpareFieldByProjectIDSys(int id);
        int CreateSpareField(IEnumerable<SpareField> resign);
        IEnumerable<SpareField> GetSpareFieldByTableName(string TableName);
        bool UpdateSpareField(IEnumerable<SpareField> resign);
        bool DeleteSpareField(int id);
    }
}
