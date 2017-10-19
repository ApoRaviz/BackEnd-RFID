using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.ImportManagement
{
    [Table("ImportDefinitionDetail_MT")]
    public class ImportDefinitionDetail_MT
    {
        [Key]
        public int ImportIDSys { get; set; }
        public string ColumnName { get; set; }
        public string Mandatory { get; set; }
        public string FixedValue { get; set; }
        public string Import { get; set; }
        public int Digits { get; set; }
        public string DataType { get; set; }

        public virtual ImportDefinitionHeader_MT ImportDefinitionHeader_MT { get; set; }
    }
}
