using HRMS.Repository.Entity.LeaveRequest;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Status;

namespace HRMS.Repository.Context
{
    public class HRMSDbContext : DbContext
    {
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveDetail> LeaveDetails { get; set; }
        public DbSet<Status_MT> Status_MT { get; set; }

        public HRMSDbContext() : base("name=HRMS")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
