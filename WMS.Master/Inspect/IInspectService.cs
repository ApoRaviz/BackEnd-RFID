using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master.Inspect
{
    public interface IInspectService
    {
        IEnumerable<InspectType> GetInspectTypes();
        IEnumerable<Inspect_MT> GetInspects();
        Inspect_MT GetInspectBySupIDSys(int id);
        int CreateInspect(Inspect_MT Inspect);
        bool UpdateInspect(int id, Inspect_MT Inspect);
        bool DeleteInspect(int id);        
    }
}
