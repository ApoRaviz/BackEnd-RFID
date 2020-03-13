using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuInboundReport
    {
        public string ID { get; set; }
        public string InvNo { get; set; }
        public int? SeqNo { get; set; }
        public string ITAOrder { get; set; }
        public string RFIDTag { get; set; }
        public string ISZJOrder { get; set; }
        public string PartNo { get; set; }
        public string ParrtName { get; set; }
        public int? Qty { get; set; }
        public string Vendor { get; set; }
        public string Shelf { get; set; }
        public string Destination { get; set; }
        public string CartonNo { get; set; }
        public string CaseNo { get; set; }

        public decimal? Weight1 { get; set; }
        public decimal? Weight2 { get; set; }
        public decimal? Weight3 { get; set; }
        public decimal? Weight4 { get; set; }
        public decimal? Weight5 { get; set; }
        public string Status { get; set; }
        public string RegisterLocation { get; set; }
        public string ReceiveLocation { get; set; }

        public DateTime? WeightDate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? PackCaseDate { get; set; }
        public DateTime? PackCartonDate { get; set; }
        public DateTime? HoldDate { get; set; }
        public DateTime? ShippingDate { get; set; }

        public int? DeliveryType { get; set; }
        public string StatusDestination { get; set; }
        public string LoadingDate { get; set; }
        public string TT { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }
        public string BL { get; set; }
        public string StatusRemark { get; set; }
        public string StatusFix { get; set; }
        public string MBL { get; set; }
        public string MNO { get; set; }
        public string HNO { get; set; }
        public string FNO { get; set; }
    }
}
