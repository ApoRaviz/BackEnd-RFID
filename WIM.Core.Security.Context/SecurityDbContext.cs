﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using WIM.Core.Security.Entity;

namespace WIM.Core.Security.Context
{    
    public class SecurityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public SecurityDbContext()
            : base("DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static SecurityDbContext Create()
        {
            return new SecurityDbContext();
        }

        //public DbSet<MenuProjectMapping> MenuProjectMapping { get; set; }
        //public DbSet<Menu> Menu { get; set; }
        public DbSet<PasswordHistory> PasswordHistory { get; set; }
        //public DbSet<Api_MT> Api_MT { get; set; }

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

            //modelBuilder.Entity<Menu>().ToTable("Menu_MT");
            //modelBuilder.Entity<Api_MT>().ToTable("Api_MT");
            //modelBuilder.Entity<MenuProjectMapping>().ToTable("MenuProjectMapping").HasKey(c => new { c.MenuIDSys, c.ProjectIDSys });
            //modelBuilder.Entity<ApiMenuMapping>().ToTable("ApiMenuMapping").HasKey(c => new { c.ApiIDSys, c.MenuIDSys });
            modelBuilder.Entity<PasswordHistory>().ToTable("PasswordHistory");
        }
    }
}