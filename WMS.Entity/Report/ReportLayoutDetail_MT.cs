using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.Report
{
    [Table("ReportLayoutDetail_MT")]
    public class ReportLayoutDetail_MT : BaseEntity
    {
        [Key]
        [Column(Order = 0)]
        public int ReportIDSys { get; set; }
        [Key]
        [Column(Order = 1)]
        public string ColumnName { get; set; }
        public Nullable<int> ColumnOrder { get; set; }

        public virtual ReportLayoutHeader_MT ReportLayoutHeader_MT { get; set; }
    }
}
