using HRMS.Entity.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
//using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace HRMS.Context
{
    public class HrmsDbContext : DbContext
    {
       
        public DbSet<Leave> Leave { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<LeaveDetail> LeaveDetail { get; set; }
        public DbSet<LeaveHistory> LeaveHistory { get; set; }
        public DbSet<LeaveQuota> LeaveQuota { get; set; }
        public DbSet<DayOff> DayOff { get; set; }


        public HrmsDbContext() : base("name=DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static HrmsDbContext Create()
        {
            return new HrmsDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}
