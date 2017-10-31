using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface IUnitService
    {
        IEnumerable<Unit_MT> GetUnits();
        Unit_MT GetUnitByUnitIDSys(int id);
        int CreateUnit(Unit_MT Unit,string username);
        bool UpdateUnit(Unit_MT Unit,string username);
        bool DeleteUnit(int id);
    }
}
