using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.WarehouseManagement
{

    [Table("Location_MT")]
    public class Location_MT
    {
        [Key]
        public int LocIDSys { get; set; }
        public string LineID { get; set; }
        public string WHID { get; set; }
        public string LocNo { get; set; }
        public string QualityType { get; set; }
        public string RackType { get; set; }
        public string Tempature { get; set; }
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }

        // #JobComment
        //public int CateIDSys { get; set; }

        //public virtual Category_MT Category_MT { get; set; }
    }
}
