using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fuji.WebApi.Models;

namespace Fuji.WebApi.Auth
{
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            this.Permissions = new HashSet<Permission>();
        }

        public bool IsSysAdmin { get; set; }

        public ICollection<Permission> Permissions { get; set; }

        public bool IsPermissionInRole(string _permission)
        {
            bool _retVal = false;
            try
            {
                foreach (Permission _perm in this.Permissions)
                {
                    if (_perm.PermissionName == _permission)
                    {
                        _retVal = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return _retVal;
        }

    }
}