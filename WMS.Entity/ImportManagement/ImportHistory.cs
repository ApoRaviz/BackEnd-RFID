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
    [Table("ImportHistory")]
    public class ImportHistory : BaseEntity
    {
        [Key]
        [Column(Order = 0)]
        public int ImportHistoryIDSys { get; set; }
        public int ProjectIdSys { get; set; }
        public int ImportDefHeadIDSys { get; set; }
        public string FileName { get; set; }
        public string Result { get; set; }
        public bool Success { get; set; }

    }
}
