using WIM.Core.Repository.Impl;
using Fuji.Context;
using Fuji.Entity.StockManagement;
using Fuji.Repository.StockManagement;

namespace Fuji.Repository.Impl.StockManagement
{
    public class CheckStockRepository : Repository<CheckStockHead>, ICheckStockRepository
    {
        FujiDbContext Db { get; set; }

        public CheckStockRepository(FujiDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
