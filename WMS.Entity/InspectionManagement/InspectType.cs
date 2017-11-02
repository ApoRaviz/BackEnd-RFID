using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.InspectionManagement
{
    [Table("InspectType")]
    public class InspectType : BaseEntity
    {
        [Key]
        public byte InspectTypeIDSys { get; set; }
        public string InspectTypeName { get; set; }

    }
}
