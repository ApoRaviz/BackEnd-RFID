using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject.CheckStock
{
    public class FujiStockReportDetail
    {
        public string DetailID { get; set; }
        public string Location { get; set; }
        public string ItemCode { get; set; }
        public string SerialNumber { get; set; }
        public string BoxNumber { get; set; }
        public string ItemGroup { get; set; }

        public int SystemQty { get; set; }
        public int CountQty { get; set; }
        public int VarianceQty {
            get {
                return Math.Abs(this.SystemQty - this.CountQty);
            }
            set { } }
        public DateTime CheckedAt { get; set; }
        public string CheckedBy { get; set; }
    }
}
