using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Validation;
using WMS.Master;

namespace WMS.Master.Report
{
    public class ReportService : IReportService
    {
        string pXmlDetail = "<row><ColumnName>{0}</ColumnName><ColumnOrder>{1}</ColumnOrder></row>";

        private MasterContext Db = MasterContext.Create();

        public List<ReportLayoutHeader_MT> GetAllReportHeader(string forTable)
        {
            List<ReportLayoutHeader_MT> report = Db.ReportLayoutHeader_MT.Where(x => x.ForTable == forTable).ToList();
            return report;
        }

        public ReportLayoutHeader_MT GetReportLayoutByReportIDSys(int id, string include)
        {
            ReportLayoutHeader_MT report = Db.ReportLayoutHeader_MT.Find(id);
            if (string.IsNullOrEmpty(include))
            {
                return report;
            }

            string[] includes = include.Replace(" ", "").Split(',');
            foreach (string inc in includes)
            {
                Db.Entry(report).Collection(inc).Load();
            }

            return report;
        }

        public int? CreateReportForItemMaster(ReportLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            int? ReportSysID = 0;

            int orderNo = 1;

            foreach (ReportLayoutDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.ColumnName, orderNo++);

            using (var scope = new TransactionScope())
            {
                data.CreatedDate = DateTime.Now;
                data.UpdatedDate = DateTime.Now;
                data.UserUpdate = "1";

                //Repo.Insert(customer);
                try
                {
                    ReportSysID = Db.ProcCreateReportLayout(data.ForTable, data.FormatName, data.FormatType, data.FileExtention, data.Delimiter, data.TextGualifier
                                              , data.Encoding, data.StartExportRow, data.IncludeHeader, data.AddHeaderLayout, data.HeaderLayout
                                              , data.CreatedDate, data.UpdatedDate, data.UserUpdate, sb.ToString()).FirstOrDefault();
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return ReportSysID;
            }
        }

        public bool UpdateReportForItemMaster(int ReportIDSys, ReportLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            int orderNo = 1;

            foreach (ReportLayoutDetail_MT d in data.detail)
                sb.AppendFormat(pXmlDetail, d.ColumnName, orderNo++);

            using (var scope = new TransactionScope())
            {
                data.UpdatedDate = DateTime.Now;
                data.UserUpdate = "1";

                try
                {
                    Db.ProcUpdateReportLayout(data.ReportIDSys, data.FormatName, data.FormatType, data.FileExtention, data.Delimiter, data.TextGualifier
                                              , data.Encoding, data.StartExportRow, data.IncludeHeader, data.AddHeaderLayout, data.HeaderLayout
                                              , data.CreatedDate, data.UpdatedDate, data.UserUpdate, sb.ToString());
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return true;
            }
        }

        public DataTable GetReportData(int ReportIDSys)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(Db.Database.Connection);

            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = Db.Database.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.ProcGetReportData";

                DbParameter param = cmd.CreateParameter();
                param.ParameterName = "@ReportID";
                param.Value = ReportIDSys;

                cmd.Parameters.Add(param);
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }
    }
}
