using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Address
{
    [Table("SubCity_MT")]
    public class SubCity_MT : BaseEntity
    {
        [Key]
        public int SubCityIDSys { get; set; }
        public string SubCityName { get; set; }
        public string SubCityNameEn { get; set; }
        public string PostalCode { get; set; }
        public int CityIDSys { get; set; }
    }
}
