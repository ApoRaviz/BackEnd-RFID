using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fuji.Entity.ItemManagement;
using Fuji.Entity.LabelManagement;
using Fuji.Entity.ProgramVersion;
using System.Data.SqlClient;
using System.Data;
using Fuji.Common.SqlLog;
using WIM.Core.Entity.Logs;
using Fuji.Entity.StockManagement;

namespace Fuji.Context
{
    public class FujiDbContext : DbContext
    {
        public DbSet<ImportSerialHead> ImportSerialHead { get; set; }
        public DbSet<ImportSerialDetail> ImportSerialDetail { get; set; }
        public DbSet<ImportSerialDetailTemp> ImportSerialDetailTemp { get; set; }
        public DbSet<LabelRunning> LabelRunning { get; set; }
        public DbSet<ProgramVersionHistory> ProgramVersionHistory { get; set; }
        public DbSet<CheckStockHead> CheckStockHead { get; set; }
        public DbSet<GeneralLog> GeneralLogs { get; set; }


        public FujiDbContext(string message = "",[System.Runtime.CompilerServices.CallerMemberName] string methodName = "") : base("name=YUT_FUJI")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        
            if (!string.IsNullOrEmpty(methodName))
            {
                string projectName = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location).Split('.').FirstOrDefault();
                string className = new System.Diagnostics.StackFrame(1)?.GetMethod()?.ReflectedType.Name;
                this.Database.Log = s => LogWriter.WritetoFile(projectName
                   , className + "." + methodName
                   , s);

            }
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

        public string ProcGetDataAutoComplete(string columnNames,string tableName,string conditionColumnNames,string keyword)
        {
            return this.Database.SqlQuery<string>("ProcGetDataAutoComplete @columnNames, @tableName, @conditionColumnNames, @keyword"
                , new SqlParameter("@columnNames", columnNames)
                , new SqlParameter("@tableName", tableName)
                , new SqlParameter("@conditionColumnNames", conditionColumnNames)
                , new SqlParameter("@keyword", keyword)).FirstOrDefault();
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

        public string ProcGetRFIDInfo(string specialQuery)
        {
            return this.Database.SqlQuery<string>("ProcGetRFIDInfo @SpeacialQuery", new SqlParameter("@SpeacialQuery", specialQuery)).FirstOrDefault();
        }

    }
}
