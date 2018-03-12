using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace WMS.Entity.SpareField
{
    [Table("SpareFields")]
    public class SpareField : BaseEntity
    {
        [Key]
        public  int SpfIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string Text { get; set; }
        public string TableName { get; set; }
        public string Type { get; set; }
    }
}
