using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Entity.InspectionManagement;

namespace WMS.Service.Inspect
{
    public interface IInspectService : IService
    {
        IEnumerable<InspectType> GetInspectTypes();
        IEnumerable<Inspect_MT> GetInspects();
        Inspect_MT GetInspectBySupIDSys(int id);
        int CreateInspect(Inspect_MT Inspect);
        bool UpdateInspect(Inspect_MT Inspect);
        bool DeleteInspect(int id);
    }
}
