using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Master;

namespace WMS.Service
{
    public interface IDimensionService
    {
        List<DimensionLayout_MT> GetAllDimension();
        DimensionLayout_MT GetDimensionLayoutByDimensionIDSys(int id);
        List<string> GetColorInSystem(int? id);
        int? CreateDimensionOfLocation(DimensionLayout_MT data);
        int? UpdateDimensionOfLocation(int DimensionIDSys, DimensionLayout_MT data);
        List<DimensionLayout_MT> GetBlock();
    }
}
