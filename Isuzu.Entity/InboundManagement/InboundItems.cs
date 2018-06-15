using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Common.Utility.Attributes;
using WIM.Core.Entity;

namespace Isuzu.Entity
{
    [Table("InboundItems")]
    public partial class InboundItems : BaseEntity 
    {
        [Key]
        public string ID { get; set; }

        [StringLength(50)]
        public string InvNo { get; set; }

        public int? SeqNo { get; set; }

        [StringLength(50)]
        public string ITAOrder { get; set; }

        [StringLength(100)]
        public string RFIDTag { get; set; }

        [StringLength(50)]
        public string ISZJOrder { get; set; }

        [StringLength(50)]
        public string PartNo { get; set; }

        [StringLength(500)]
        public string ParrtName { get; set; }

        public int? Qty { get; set; }

        [StringLength(250)]
        public string Vendor { get; set; }

        [StringLength(50)]
        public string Shelf { get; set; }

        [StringLength(50)]
        public string Destination { get; set; }

        [StringLength(50)]
        public string CartonNo { get; set; }

        [StringLength(50)]
        public string CaseNo { get; set; }

        public decimal Weight1 { get; set; }
        public decimal Weight2 { get; set; }
        public decimal Weight3 { get; set; }
        public decimal Weight4 { get; set; }
        public decimal Weight5 { get; set; }

        [NotMapped]
        public int? WeightCursor {
            get {
                int ret = 1;
                ret += this.Weight1 > 0 ? 1 : 0;
                ret += this.Weight2 > 0 ? 1 : 0;
                ret += this.Weight3 > 0 ? 1 : 0;
                ret += this.Weight4 > 0 ? 1 : 0;
                ret += this.Weight5 > 0 ? 1 : 0;
                return ret;
            }
            set { }
        }

        [GeneralLog]
        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(10)]
        public string RegisterLocation { get; set; }

        [StringLength(10)]
        public string ReceiveLocation { get; set; }

        public DateTime? WeightDate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? PackCaseDate { get; set; }
        public DateTime? PackCartonDate { get; set; }
        public DateTime? HoldDate { get; set; }
        public DateTime? ShippingDate { get; set; }

        [StringLength(50)]
        public string DeleteReason { get; set; }
        
        [StringLength(100)]
        public string PathDeleteReason { get; set; }

        public virtual InboundItemsHead InboundItemsHead { get; set; }
    }
}
