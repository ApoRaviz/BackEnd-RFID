using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master
{
    public class RoleUserDto
    {
        public string RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSysAdmin { get; set; }
        public Project_MT Project_MT { get; set; }
        public List<UserRoleDto> Users { get; set; }
    }
}
