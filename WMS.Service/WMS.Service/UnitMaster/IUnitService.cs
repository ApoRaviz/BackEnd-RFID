using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{
    public interface IUnitService
    {
        IEnumerable<ProcGetUnits_Result> GetUnits();        
        ProcGetUnitByUnitIDSys_Result GetUnitByUnitIDSys(int id);
        int CreateUnit(Unit_MT Unit);
        bool UpdateUnit(int id, Unit_MT Unit);
        bool DeleteUnit(int id);
    }
}
