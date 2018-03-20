using Fuji.Common.ValueObject;
using Fuji.Entity.StockManagement;
using System.Collections.Generic;
using WIM.Core.Service;

namespace Fuji.Service.ItemImport
{
    public interface ICheckStockService : IService
    {

        bool ImportCheckStock();
        CheckStockHead GetStockHeadByID(string StockID);
        IEnumerable<CheckStockHead> GetStock(int pageIndex, int pageSize, out int totalRecord);
        IEnumerable<CheckStockHead> SearchStockBy(ParameterSearch parameterSearch);

    }
}
