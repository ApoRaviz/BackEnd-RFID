using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WIM.Core.Entity.ProjectManagement;

namespace Auth.Security.Entity.UserManagement
{
    //[Table("UserProjectMapping")]
    public class UserProjectMapping
    {
        [Key]
        [Column(Order = 1)]
        public string UserID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ProjectIDSys { get; set; }

        //public virtual Project_MT Project_MT { get; set; }
        public virtual User User { get; set; }
    }
}
