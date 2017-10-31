using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Isuzu.Entity;

namespace Isuzu.Context
{
    public class IsuzuDataContext : DbContext
    {

        public virtual DbSet<InboundItems> InboundItems { get; set; }
        public virtual DbSet<InboundItemsHead> InboundItemsHead { get; set; }
        public virtual DbSet<LabelRunning> LabelRunning { get; set; }

        public IsuzuDataContext() : base("name=WIM_ISUZU"){
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static IsuzuDataContext Create()
        {
            return new IsuzuDataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
