using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Security.Entity.UserManagement
{
    [Table("PasswordHistory")]
    public class PasswordHistory
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
