using Fuji.Entity.ItemManagement;
using Fuji.Entity.LabelManagement;
using Fuji.Entity.ProgramVersion;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Context
{
    public class FujiDbContext : DbContext
    {
        public DbSet<ImportSerialHead> ImportSerialHead { get; set; }
        public DbSet<ImportSerialDetail> ImportSerialDetail { get; set; }
        public DbSet<LabelRunning> LabelRunning { get; set; }
        public DbSet<ProgramVersionHistory> ProgramVersionHistory { get; set; }

        public FujiDbContext() : base("name=WIM_FUJI")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static FujiDbContext Create()
        {
            return new FujiDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ImportSerialHead>().ToTable("ImportSerialHead");
            //modelBuilder.Entity<ImportSerialDetail>().ToTable("ImportSerialDetail");

        }

        public ObjectResult<string> ProcGetNewID(string prefixes)
        {
            var prefixesParameter = prefixes != null ?
                new ObjectParameter("Prefixes", prefixes) :
                new ObjectParameter("Prefixes", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ProcGetNewID", prefixesParameter);
        }

    }
}
