using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Fuji.WebApi.Auth;

namespace Fuji.WebApi.Models
{
    [Table("Permissions")]
    public class Permission
    {

        public Permission()
        {
            this.Roles = new HashSet<ApplicationRole>();
        }

        [Key]
        public string PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string Method { get; set; }
        public Nullable<int> MenuIDSys { get; set; }
        public Nullable<int> ProjectIDSys { get; set; }

        public ICollection<ApplicationRole> Roles { get; set; }        

        public MenuProjectMapping MenuProjectMapping { get; set; }
    }


    

}