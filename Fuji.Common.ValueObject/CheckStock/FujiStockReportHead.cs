using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject.CheckStock
{
    public class FujiStockReportHead
    {
        public string StockID { get; set; }
        public string CreateAt { get; set; }
        public string CreateBy { get; set; }
        public string Location { get; set; }
        public string WarehouseCode { get; set; }
        public int Qty { get; set; }

        public IEnumerable<FujiStockReportDetail> Details { get; set; }

    }
}
