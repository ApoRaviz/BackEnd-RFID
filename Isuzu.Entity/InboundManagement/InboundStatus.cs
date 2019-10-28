using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Common.Utility.Attributes;
using WIM.Core.Entity;

namespace Isuzu.Entity
{
    [Table("InboundStatus")]
    public partial class InboundStatus : BaseEntity 
    {
        [Key]
        public string ID { get; set; }

        [StringLength(50)]
        public string InvNo { get; set; }

        [StringLength(20)]
        public string StatusName { get; set; }

        [StringLength(250)]
        public string StatusDetail { get; set; }


    }
}
