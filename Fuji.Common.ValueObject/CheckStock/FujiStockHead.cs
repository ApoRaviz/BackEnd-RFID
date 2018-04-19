using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject.CheckStock
{
    public class FujiStockHead
    {
        public string StockID { get; set; }
        public int SystemQty { get; set; }
        public int CountQty { get; set; }
        public int VarianceQty { get; set; }
        public DateTime CheckedAt { get; set; }
        public string CheckedBy { get; set; }
    }
}
