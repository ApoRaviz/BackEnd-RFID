using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WIM.Core.Entity.CustomerManagement;

namespace WIM.Core.Security.Entity.UserManagement
{
    //[Table("UserCustomerMapping")]
    public partial class UserCustomerMapping
    {
        
        public string UserID { get; set; }
        public int CusIDSys { get; set; }
        [Key]
        public int IDSys { get; set; }

        public virtual User User { get; set; }
        //public virtual Customer_MT Customer_MT { get; set; }
    }
}
