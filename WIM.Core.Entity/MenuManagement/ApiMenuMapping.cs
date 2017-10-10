using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.MenuManagement
{
    [Table("ApiMenuMapping")]
    public class ApiMenuMapping
    {
        [Key]
        public string ApiIDSys { get; set; }
        [Key]
        public int MenuIDSys { get; set; }

        public Api_MT Api { get; set; }
    }
}
