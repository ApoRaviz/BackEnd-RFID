using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace Fuji.Entity.ItemManagement
{
    [Table("ImportSerialDetail")]
    public partial class ImportSerialDetail : BaseEntity
    {
        [Key]
        [StringLength(50)]
        public string DetailID { get; set; }        
        public string ItemCode { get; set; }
        public string SerialNumber { get; set; }
        public string BoxNumber { get; set; }
        public string ItemGroup { get; set; }
        public string ItemType { get; set; }
        public string Status { get; set; }
        public string OrderNo { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public string UserUpdate { get; set; }
        [StringLength(50)]
        public string HeadID { get; set; }
        public virtual ImportSerialHead ImportSerialHead { get; set; }
    }
}
