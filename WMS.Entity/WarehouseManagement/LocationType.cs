
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Entity.WarehouseManagement
{
    [Table("LocationTypes")]
    public class LocationType
    {
        [Key]
        public int LocTypeIDSys { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
