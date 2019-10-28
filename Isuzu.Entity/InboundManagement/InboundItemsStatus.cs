using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Common.Utility.Attributes;
using WIM.Core.Entity;

namespace Isuzu.Entity
{
    [Table("InboundStatus")]
    public partial class InboundItemsStatus : BaseEntity
    {
        public InboundItemsStatus()
        {

        }

        public InboundItemsStatus(string invNo, string statusName, string statusDetail): base()
        {
            InvNo = invNo;
            StatusName = statusName;
            StatusDetail = statusDetail;
        }

        public void Update(string invNo, string statusName, string statusDetail)
        {
            InvNo = invNo;
            StatusName = statusName;
            StatusDetail = statusDetail;
        }

        [Key]
        public int ID { get; set; }

        [StringLength(50)]
        public string InvNo { get; set; }

        [StringLength(20)]
        public string StatusName { get; set; }

        public string StatusDetail { get; set; }

        
    }
}
