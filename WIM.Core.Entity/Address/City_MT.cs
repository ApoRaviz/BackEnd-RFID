using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Address
{
    [Table("City_MT")]
    public class City_MT: BaseEntity
    {
        [Key]
        public int CityIDSys { get; set; }
        public string CityName { get; set; }
        public int ProvinceIDSys { get; set; }
    }
}
