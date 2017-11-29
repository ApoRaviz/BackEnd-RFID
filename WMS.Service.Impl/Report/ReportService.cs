using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Validation;
using WMS.Context;
using WMS.Entity.Report;
using WMS.Repository.Impl.Report;
using WMS.Repository.Report;
using WMS.Service.Report;

namespace WMS.Service.Impl.Report
{
    public class ReportService : WIM.Core.Service.Impl.Service, IReportService
    {
        string pXmlDetail = "<row><ColumnName>{0}</ColumnName><ColumnOrder>{1}</ColumnOrder></row>";


        public List<ReportLayoutHeader_MT> GetAllReportHeader(string forTable)
        {
            List<ReportLayoutHeader_MT> report;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IReportLayoutHeaderRepository repo = new ReportLayoutHeaderRepository(Db);
                report = repo.GetMany(x => x.ForTable == forTable).ToList();
            }
                
            return report;
        }

        public ReportLayoutHeader_MT GetReportLayoutByReportIDSys(int id, string include)
        {
            using(WMSDbContext Db = WMSDbContext.Create())
            {
                IReportLayoutHeaderRepository repo = new ReportLayoutHeaderRepository(Db);
                ReportLayoutHeader_MT report = repo.GetByID(id);
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
            

            
        }

        public int? CreateReportForItemMaster(ReportLayoutHeader_MT data)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                System.Text.StringBuilder sb = new StringBuilder();
                int? ReportSysID = 0;

                int orderNo = 1;

                foreach (ReportLayoutDetail_MT d in data.detail)
                {
                    d.IsActive = true;
                    d.CreateAt = DateTime.Now;
                    d.UpdateAt = DateTime.Now;
                    d.UpdateBy = Identity.Name;
                    sb.AppendFormat(pXmlDetail, d.ColumnName, orderNo++);
                }

                using (var scope = new TransactionScope())
                {
                    data.CreateAt = DateTime.Now;
                    data.UpdateAt = DateTime.Now;
                    data.UpdateBy = Identity.Name;
                    data.CreateBy = Identity.Name;
                    //Repo.Insert(customer);
                    try
                    {
                        ReportSysID = Db.ProcCreateReportLayout(data.ForTable, data.FormatName, data.FormatType, data.FileExtention, data.Delimiter, data.TextGualifier
                                                  , data.Encoding, data.StartExportRow, data.IncludeHeader, data.AddHeaderLayout, data.HeaderLayout
                                                  , data.CreateAt, data.UpdateAt, data.UpdateBy, sb.ToString());
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
        }

        public bool UpdateReportForItemMaster(int ReportIDSys, ReportLayoutHeader_MT data)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            int orderNo = 1;

            foreach (ReportLayoutDetail_MT d in data.detail)
            {
                d.IsActive = true;
                d.UpdateAt = DateTime.Now;
                d.UpdateBy = Identity.Name;
                sb.AppendFormat(pXmlDetail, d.ColumnName, orderNo++);
            }

            using (var scope = new TransactionScope())
            {
                    data.UpdateAt = DateTime.Now;
                    data.UpdateBy = Identity.Name;
                using (WMSDbContext Db = new WMSDbContext())
                {
                    try
                    {
                        Db.ProcUpdateReportLayout(data.ReportIDSys, data.FormatName, data.FormatType, data.FileExtention, data.Delimiter, data.TextGualifier
                                                  , data.Encoding, data.StartExportRow, data.IncludeHeader, data.AddHeaderLayout, data.HeaderLayout
                                                  , data.CreateAt, data.UpdateAt, data.UpdateBy, sb.ToString());
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
        }

        public DataTable GetReportData(int ReportIDSys)
        {
            //Oil Comment
            using (WMSDbContext Db = new WMSDbContext())
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
