using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Isuzu.Entity
{
    [Table("InboundItemsHead")]
    public partial class InboundItemsHead
    {
        public InboundItemsHead()
        {
            InboundItems = new HashSet<InboundItems>();
        }

        [Key]
        [StringLength(50)]
        public string InvNo { get; set; }

        public int? Qty { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        public bool? IsExport { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateAt { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateAt { get; set; }

        [StringLength(50)]
        public string UpdateBy { get; set; }


        public virtual ICollection<InboundItems> InboundItems { get; set; }
    }
}
