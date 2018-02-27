using System.Collections.Generic;
using WIM.Core.Service;
using WMS.Entity.SpareField;

namespace WMS.Service
{
    public interface ISpareFieldService : IService
    {
        IEnumerable<SpareField> GetSpareField();
        SpareField GetSpareFieldBySpfIDSys(int id);
        int CreateSpareField(IEnumerable<SpareField> resign);
        bool UpdateSpareField(SpareField resign);
        bool DeleteSpareField(int id);
    }
}
