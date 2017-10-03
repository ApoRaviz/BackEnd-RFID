using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Isuzu.WebApi.Models;

namespace Isuzu.WebApi.Auth
{
    public class ApplicationUserLogin : IdentityUserLogin<string> { }
    public class ApplicationUserClaim : IdentityUserClaim<string> { }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<MenuProjectMapping> MenuProjectMapping { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<PasswordHistory> PasswordHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserID");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles").Property(p => p.Id).HasColumnName("RoleID");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<ApplicationRole>().
            HasMany(c => c.Permissions).
            WithMany(p => p.Roles).
            Map(
                m =>
                {
                    m.MapLeftKey("RoleID");
                    m.MapRightKey("PermissionID");
                    m.ToTable("RolePermission");
                });
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins");

            modelBuilder.Entity<Menu>().ToTable("Menu_MT");
            modelBuilder.Entity<MenuProjectMapping>().ToTable("MenuProjectMapping").HasKey(c => new { c.MenuIDSys, c.ProjectIDSys });
            modelBuilder.Entity<PasswordHistory>().ToTable("PasswordHistory");
        }
    }
}