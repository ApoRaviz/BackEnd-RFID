using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace Isuzu.Entity
{
   

    [Table("LabelRunning")]
    public partial class LabelRunning: BaseEntity
    {
        public int ID { get; set; }

        [Required]
        [StringLength(4)]
        public string Type { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        public int Running { get; set; }

        public bool Printing { get; set; }
    }
}
