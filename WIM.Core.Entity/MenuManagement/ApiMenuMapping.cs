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
        [Column(Order = 1)]
        public string ApiIDSys { get; set; }
        [Key]
        [Column(Order = 2)]
        public int MenuIDSys { get; set; }
        public byte GET { get; set; }
        public byte POST { get; set; }
        public byte PUT { get; set; }
        public byte DEL { get; set; }
        public string Type { get; set; }

        public virtual Menu_MT Menu_MT { get; set; }
        public virtual Api_MT Api_MT { get; set; }
    }
}
