using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface IUnitService : IService
    {
        IEnumerable<Unit_MT> GetUnits();
        Unit_MT GetUnitByUnitIDSys(int id);
        int CreateUnit(Unit_MT Unit);
        bool UpdateUnit(Unit_MT Unit);
        bool DeleteUnit(int id);
        IEnumerable<AutocompleteUnitDto> AutocompleteUnit(string term);
    }
}
