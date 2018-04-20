using HRMS.Entity.Evaluate;
using HRMS.Entity.Form;
using HRMS.Entity.LeaveManagement;
using HRMS.Entity.Probation;
using System.Data.Entity;
//using System.Data.Objects;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.Status;

namespace HRMS.Context
{
    public class HRMSDbContext : DbContext
    {       
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveDetail> LeaveDetails { get; set; }
        public DbSet<LeaveHistory> LeaveHistory { get; set; }
        public DbSet<LeaveQuota> LeaveQuotas { get; set; }
        public DbSet<DayOff> DayOffs { get; set; }
        public DbSet<Status_MT> Status_MT { get; set; }
        public DbSet<Project_MT> Project_MT { get; set; }
        public DbSet<VEmployeeInfo> VEmployeeInfo { get; set; }
        public DbSet<FormQuestion> FormQuestion { get; set; }
        public DbSet<FormTopic> FormTopic { get; set; }
        public DbSet<Evaluated> Evaluated { get; set; }
        public DbSet<FormDetail> FormDetail { get; set; }

        





        public HRMSDbContext() : base("name=YUT_HRMS")
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
