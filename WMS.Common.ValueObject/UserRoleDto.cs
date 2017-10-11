using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common
{
    public class UserRoleDto
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public List<RoleUserDto> Roles { get; set; }
    }
}
