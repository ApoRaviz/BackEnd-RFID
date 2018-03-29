using Fuji.Common.ValueObject.CheckStock;
using Fuji.Entity.StockManagement;
using System.Collections.Generic;
using WIM.Core.Repository;

namespace Fuji.Repository.StockManagement
{
    public interface ICheckStockRepository : IRepository<CheckStockHead>
    {
        FujiStockReportHead GetStockDetailByLocation(FujiStockReportHead checkStockHead);
        FujiStockReportHead GetStockHeadByInprogress(FujiStockReportHead checkStockHead);
    }
}
