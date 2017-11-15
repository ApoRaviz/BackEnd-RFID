using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fuji.Entity.ItemManagement;
using Fuji.Entity.LabelManagement;
using Fuji.Entity.ProgramVersion;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data;

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
        }


        public void ProcDeleteImportSerial()
        {

        }
        public void ProcDeleteImportSerialDetail()
        {
        }
        public void ProcGetDataAutoComplete()
        {
        }
        public void ProcGetImportSerialHead()
        {
        }
        public void ProcGetImportSerialHeadByHeadID()
        {
        }
        public IEnumerable<ImportSerialDetail> ProcPagingImportSerialDetail(int page, int size, out int totalRecord)
        {
            var output = new SqlParameter("@totalrow", SqlDbType.Int, 30);
            output.Direction = ParameterDirection.Output;


            var items = this.Database.SqlQuery<ImportSerialDetail>("ProcPagingImportSerialDetail @page,@size,@totalrow out"
                , new SqlParameter("@page", page)
                , new SqlParameter("@size", size)
                , output).ToList();

            totalRecord = Convert.ToInt32(output.Value);

            return items;
        }
        public IEnumerable<ImportSerialHead> ProcPagingImportSerialHead(int page, int size, out int totalRecord, string sort = "CreateAt", string sortDecending = "DESC")
        {
            totalRecord = 0;
            var output = new SqlParameter("@totalrow", SqlDbType.Int, 30);
            output.Direction = ParameterDirection.Output;

            var items = this.Database.SqlQuery<ImportSerialHead>("ProcPagingImportSerialHead @page,@size,@sort,@sortdecending,@totalrow out"
                , new SqlParameter("@page", page)
                , new SqlParameter("@size", size)
                , new SqlParameter("@sort", sort)
                , new SqlParameter("@sortdecending", sortDecending)
                , output).ToList();

            totalRecord = Convert.ToInt32(output.Value);

            return items;
        }
        public void ProcPagingImportSerialHeadSearch()
        {
        }
        public void ProcPagingPickingOrder()
        {
        }
        public void ProcRunDeleteImportSerial()
        {
        }
        public string ProcGetNewID(string prefix)
        {
            return this.Database.SqlQuery<string>("ProcGetNewID @Prefixes",new SqlParameter("@Prefixes",prefix)).FirstOrDefault();
        }

    }
}
