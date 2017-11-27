using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(TMS.WebApi.Startup))]

namespace TMS.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureOAuthTokenConsumption(app);
            //CreateRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login
        //private void CreateRolesandUsers()
        //{
        //    ApplicationDbContext context = new ApplicationDbContext();

        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //    var userManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser>(context));
           
        //    // In Startup iam creating first Admin Role and creating a default Admin User 
        //    if (!roleManager.RoleExists("Admin"))
        //    {

        //        // first we create Admin rool
        //        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //        role.Name = "Admin";
        //        roleManager.Create(role);

        //        //Here we create a Admin super user who will maintain the website				
        //        var user = new ApplicationUser();
        //        user.UserName = "adminwms";
        //        user.Email = "admin-wms@yamatothai.com";
        //        user.Name = "Admin";
        //        user.Surname = "WMS";
        //        user.Active = 1;
        //        user.CreateDate = DateTime.Now;
        //        user.UpdateDate = DateTime.Now;
        //        user.UserUpdate = "System";

        //        string userPWD = "Zxcv123!";

        //        var chkUser = userManager.Create(user, userPWD);

        //        //Add default User to Role Admin
        //        if (chkUser.Succeeded)
        //        {
        //            var result1 = userManager.AddToRole(user.Id, "Admin");

        //        }
        //    }

        //    // creating Creating Manager role 
        //    if (!roleManager.RoleExists("Manager"))
        //    {
        //        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //        role.Name = "Manager";
        //        roleManager.Create(role);

        //    }

        //    // creating Creating Employee role 
        //    if (!roleManager.RoleExists("Employee"))
        //    {
        //        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //        role.Name = "Employee";
        //        roleManager.Create(role);

        //    }
        //}
    }
}
