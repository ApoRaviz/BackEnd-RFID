﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Auth.Security.Entity.UserManagement;

namespace Auth.Security.Entity
{
    public class ApplicationUserLogin : IdentityUserLogin<string> { }
    public class ApplicationUserClaim : IdentityUserClaim<string> { }

  
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public DateTime LastLogin { get; set; }
        public bool IsSysAdmin { get; set; }
        public Nullable<int> KeyOTP { get; set; }
        public Nullable<System.DateTime> KeyOTPDate { get; set; }
        public string TokenMobile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            //userIdentity.AddClaim(new Claim("Permission", Permission.ToString()));

            //userIdentity.RemoveClaim(userIdentity.FindFirst("role"));
            return userIdentity;
        }

        public bool IsPermissionInUserRoles(string _permission)
        {
            bool _retVal = false;
            try
            {
                foreach (ApplicationUserRole _role in this.Roles)
                {
                    if (_role.IsPermissionInRole(_permission))
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