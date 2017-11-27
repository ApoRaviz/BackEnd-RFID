using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Isuzu.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

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

        public void ProcGetNewID()
        {
        }
        public void ProcPagingInboundItemGroups()
        {
        }
        public IEnumerable<InboundItemsHead> ProcPagingInboundItemHead(int page ,int size,out int totalRecord)
        {
            totalRecord = 0;
            var output = new SqlParameter("@totalRecord", SqlDbType.Int, 30);
            output.Direction = ParameterDirection.Output;

            var items = this.Database.SqlQuery<InboundItemsHead>("ProcPagingInboundItemHead @page,@size,@totalRecord out"
                , new SqlParameter("@page", page)
                , new SqlParameter("@size", size)
                , output).ToList();

            totalRecord = Convert.ToInt32(output.Value);

            return items;
        }
        public IEnumerable<InboundItems> ProcPagingInboundItems(int page, int size, out int totalRecord)
        {
            totalRecord = 0;
            var output = new SqlParameter("@totalRecord", SqlDbType.Int, 30);
            output.Direction = ParameterDirection.Output;

            var items = this.Database.SqlQuery<InboundItems>("ProcPagingInboundItems @page,@size,@totalRecord out"
                , new SqlParameter("@page", page)
                , new SqlParameter("@size", size)
                , output).ToList();

            totalRecord = Convert.ToInt32(output.Value);

            return items;
        }
        public IEnumerable<InboundItems> ProcPagingInboundItemsDeleted(int page, int size, out int totalRecord)
        {
            totalRecord = 0;
            var output = new SqlParameter("@totalRecord", SqlDbType.Int, 30);
            output.Direction = ParameterDirection.Output;

            var items = this.Database.SqlQuery<InboundItems>("ProcPagingInboundItemsDeleted @page,@size,@totalRecord out"
                , new SqlParameter("@page", page)
                , new SqlParameter("@size", size)
                , output).ToList();

            totalRecord = Convert.ToInt32(output.Value);

            return items;
        }
    }
}
