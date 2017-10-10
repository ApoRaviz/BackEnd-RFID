using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Context
{

    public class WMSDbContext : DbContext
    {
        /*public virtual DbSet<Unit_MT> Unit_MT { get; set; }*/

        public WMSDbContext() : base("name=WIM_FUJI")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static WMSDbContext Create()
        {
            return new WMSDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}
