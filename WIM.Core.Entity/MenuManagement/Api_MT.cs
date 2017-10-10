using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.MenuManagement
{
    [Table("Api_MT")]
    public class Api_MT
    {
        public Api_MT()
        {
            this.ApiMenuMappings = new HashSet<ApiMenuMapping>();
            //#JobComment
            //this.Permissions = new HashSet<Permission>();
        }

        [Key]
        public string ApiIDSys { get; set; }
        public string Api { get; set; }
        public string Method { get; set; }
        public string Controller { get; set; }

        public virtual ICollection<ApiMenuMapping> ApiMenuMappings { get; set; }
        //public virtual ICollection<Permission> Permissions { get; set; }
    }
}
