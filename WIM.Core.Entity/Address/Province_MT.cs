using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Address
{
    [Table("Province_MT")]
    public class Province_MT : BaseEntity
    {
        [Key]
        public int ProvinceIDSys { get; set; }
        public string ProvinceName { get; set; }
        public int CountryIDSys { get; set; }
    }
}
