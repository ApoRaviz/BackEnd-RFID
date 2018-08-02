using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.ImportManagement
{
    [Table("ImportDefinitionDetail_MT")]
    public class ImportDefinitionDetail_MT : BaseEntity
    {
        [Key]
        [Column(Order = 0)]
        public int ImportDefDetailIDSys { get; set; }
        [Key]
        [Column(Order = 1)]
        public int ImportDefHeadIDSys { get; set; }
        [Key]
        [Column(Order = 2)]
        public string ColumnName { get; set; }
        public string Mandatory { get; set; }
        public string FixedValue { get; set; }
        public string Import { get; set; }
        public bool IsHead { get; set; }
        public bool IsRefKey { get; set; }
        public int Digits { get; set; }
        public string DataType { get; set; }

        [ForeignKey("ImportDefHeadIDSys")]
        public virtual ImportDefinitionHeader_MT ImportDefinitionHeader_MT { get; set; }
    }
}
