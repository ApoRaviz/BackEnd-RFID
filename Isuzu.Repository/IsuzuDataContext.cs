namespace Isuzu.Repository
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class IsuzuDataContext : DbContext
    {
        public IsuzuDataContext()
            : base("name=IsuzuDataContext")
        {
        }

        public virtual DbSet<InboundItems> InboundItems { get; set; }
        public virtual DbSet<InboundItemsHead> InboundItemsHead { get; set; }
        public virtual DbSet<LabelRunning> LabelRunning { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LabelRunning>()
                .Property(e => e.Type)
                .IsUnicode(false);
        }
    }
}
