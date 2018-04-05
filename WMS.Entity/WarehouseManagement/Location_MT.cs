using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;
using WMS.Entity.ItemManagement;

namespace WMS.Entity.WarehouseManagement
{

    [Table("Location_MT")]
    public class Location_MT : BaseEntity
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
        public int CateIDSys { get; set; }

        [ForeignKey("CateIDSys")]
        public virtual Category_MT Category_MT { get; set; }
    }
}
