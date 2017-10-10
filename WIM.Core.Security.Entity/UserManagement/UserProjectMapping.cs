using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;

namespace WIM.Core.Security.Entity.UserManagement
{
    public class UserProjectMapping
    {
        public string UserID { get; set; }
        public int ProjectIDSys { get; set; }

        public virtual Project_MT Project_MT { get; set; }
        public virtual User User { get; set; }
    }
}
