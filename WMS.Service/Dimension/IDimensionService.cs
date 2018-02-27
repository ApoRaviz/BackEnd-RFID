using System;
using System.Collections.Generic;
using System.Linq;
using WIM.Core.Service;
using WMS.Entity.Dimension;

namespace WMS.Service
{
    public interface IDimensionService : IService
    {
        List<DimensionLayout_MT> GetAllDimension();
        DimensionLayout_MT GetDimensionLayoutByDimensionIDSys(int id);
        List<string> GetColorInSystem(int? id);
        int? CreateDimensionOfLocation(DimensionLayout_MT data);
        int? UpdateDimensionOfLocation(int DimensionIDSys, DimensionLayout_MT data);
        List<DimensionLayout_MT> GetBlock();
    }
}
