using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace WIM.Core.Entity.TableControl
{
    [Table("TableControls")]
    public class TableControl : BaseEntity
    {
        [Key]
        public int TableControlIDsys { get; set; }
        public string TableName { get; set; }
        public string TextDisplay { get; set; }
    }
}
