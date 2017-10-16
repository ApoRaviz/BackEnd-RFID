using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.InspectionManagement
{
    [Table("InspectType")]
    public class InspectType
    {
        [Key]
        public byte InspectTypeIDSys { get; set; }
        public string InspectTypeName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }
    }
}
