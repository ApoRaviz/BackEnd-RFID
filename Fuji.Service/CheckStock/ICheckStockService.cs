using Fuji.Common.ValueObject;
using Fuji.Entity.StockManagement;
using System.Collections.Generic;
using WIM.Core.Service;

namespace Fuji.Service.ItemImport
{
    public interface ICheckStockService : IService
    {
        CheckStockHead CreateCheckStockHead();
        bool UpdateCheckStockHead(CheckStockHead checkStockHead);
        CheckStockHead GetStockHeadByID(string checkStockID);
        CheckStockHead GetStockHeadByProgress();
        IEnumerable<CheckStockHead> GetStock(int pageIndex, int pageSize, out int totalRecord);
        IEnumerable<CheckStockHead> SearchStockBy(ParameterSearch parameterSearch);

    }
}
