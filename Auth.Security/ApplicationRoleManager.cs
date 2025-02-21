﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Auth.Security.Context;
using Auth.Security.Entity;

namespace Auth.Security
{
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<SecurityDbContext>()));

            return appRoleManager;
        }        

        public static ApplicationRole GetRole(string roleID)
        {
            ApplicationRole role = null;
            // #JobComment
            using (RoleStore<ApplicationRole, string, ApplicationUserRole> db = new RoleStore<ApplicationRole, string, ApplicationUserRole>(new SecurityDbContext()))
            {
                role = db.Roles.Where(r => r.Id == roleID)
                    .Include(p => p.Permissions.Select(mp => mp.MenuProjectMapping.Menu_MT)
                    ).FirstOrDefault();

            }
            return role;
        }

 

        public static ApplicationRole GetRolePermissions(string roleID)
        {
            ApplicationRole role = null;
            using (RoleStore<ApplicationRole, string, ApplicationUserRole> db = new RoleStore<ApplicationRole, string, ApplicationUserRole>(new SecurityDbContext()))
            {
                role = (db.Roles.Where(r => r.Id == roleID)
                    .Include(p => p.Permissions.Select(mp => mp.MenuProjectMapping.Menu_MT)
                    ).Include(p => p.Permissions.Select(mp => mp.Api_MT)
                    )).FirstOrDefault();
                Console.Write(role);
            }
            return role;
        }
    }
}