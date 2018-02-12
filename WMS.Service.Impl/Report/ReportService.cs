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
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Entity.Report;
using WMS.Repository.Impl.Report;
using WMS.Repository.Report;
using WMS.Service.Report;

namespace WMS.Service.Impl.Report
{
    public class ReportService : WIM.Core.Service.Impl.Service, IReportService
    {
        string pXmlDetail = "<row><ColumnName>{0}</ColumnName><ColumnOrder>{1}</ColumnOrder></row>";


        public List<ReportLayout_MT> GetAllReportHeader(int ProjectIDSys)
        {
            List<ReportLayout_MT> report;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IReportLayoutRepository repo = new ReportLayoutRepository(Db);
                if(ProjectIDSys == 0)
                {
                    report = repo.Get().ToList();
                }
                else
                {
                    report = repo.GetMany(x => x.ProjectIDSys == ProjectIDSys).ToList();
                }
                
            }
                
            return report;
        }

        public ReportLayout_MT GetReportLayoutByReportIDSys(int id)
        {
            using(WMSDbContext Db = WMSDbContext.Create())
            {
                IReportLayoutRepository repo = new ReportLayoutRepository(Db);
                ReportLayout_MT report = repo.GetByID(id);
                return report;
            }
        }

        public int CreateReportForItemMaster(ReportLayout_MT data)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                IReportLayoutRepository repo = new ReportLayoutRepository(Db);

                WhereTable(data.ReportDetail.Detail, data.ReportDetail.Where);
                using (var scope = new TransactionScope())
                {
                    ReportLayout_MT newData = new ReportLayout_MT();
                    try
                    {
                        newData = repo.Insert(data);
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                    catch (DbUpdateException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(ErrorEnum.E4012);
                        throw ex;
                    }
                    scope.Complete();
                    return newData.ReportIDSys;
                }
            }
        }

        public bool UpdateReportForItemMaster(int ReportIDSys, ReportLayout_MT data)
        {
            using (var scope = new TransactionScope())
            {
                    data.UpdateAt = DateTime.Now;
                    data.UpdateBy = Identity.Name;
                WhereTable(data.ReportDetail.Detail, data.ReportDetail.Where);
                using (WMSDbContext Db = new WMSDbContext())
                {
                    ReportLayout_MT newData = new ReportLayout_MT();
                    IReportLayoutRepository repo = new ReportLayoutRepository(Db);

                    try
                    {
                        newData = repo.Update(data);
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                    catch (DbUpdateException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(ErrorEnum.E4012);
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
                ReportLayoutRepository repo = new ReportLayoutRepository(Db);
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(Db.Database.Connection);

                using (var cmd = dbFactory.CreateCommand())
                {
                    var data = repo.GetByID(ReportIDSys);
                    List<TableColumnDetail> tablecolumn = new List<TableColumnDetail>();
                    string tablename = " ";
                    foreach(var table in data.ReportDetail.Detail)
                    {
                        foreach (var column in table.Column)
                        {
                            TableColumnDetail newcolumn = new TableColumnDetail();
                            newcolumn.TableName = table.TableName;
                            newcolumn.ColumnName = column.ColumnName;
                            newcolumn.AliasName = column.AliasName;
                            newcolumn.ColumnOrder = column.ColumnOrder;
                            tablecolumn.Add(newcolumn);
                        }
                        tablename += table.TableName + ",";
                    }
                    tablecolumn = tablecolumn.OrderBy(a => a.ColumnOrder).ToList();

                    string Query = "Select ";

                    foreach (var column in tablecolumn)
                    {
                        Query += column.TableName + "." + column.ColumnName + " As '"+column.AliasName +"' ,";
                    }

                    if(data.ReportDetail.Operator.Count > 0)
                    {
                        foreach(var oper in data.ReportDetail.Operator)
                        {
                            Query += oper.ColumnFirst.ColumnName + ' ' + oper.Operator + ' ' + oper.ColumnSecond.ColumnName + " As '" + oper.AliasName + "' ,";
                        }
                    }
                    Query = Query.Substring(0,Query.Length-1) + " From " + JoiningTable(data.ReportDetail.Detail);
                    string[] likecondition = { "like", "not like" };
                        if (data.ReportDetail.Filter != null)
                        {
                            string where = " Where ";
                            foreach (var were in data.ReportDetail.Filter)
                            {
                                string condition;
                            if (were.Operator.Equals("between"))
                            {
                                if (were.Condition2 != null && were.Condition1 != null)
                                {
                                    condition = were.ColumnName + " " + were.Operator + " '" + were.Condition1 + "' and '" + were.Condition2 + " ";
                                    where += " " + condition + " And";
                                }
                            }
                            else if (likecondition.Contains(were.Operator) && were.Condition1 != null)
                                {
                                    condition = were.ColumnName + " " + were.Operator + " '%" + were.Condition1 +"%' ";
                                    where += " " + condition + " And";
                                }
                                else if (were.Condition1 != null)
                                {
                                    condition = were.ColumnName + " " + were.Operator + " '" + were.Condition1 +"' ";
                                    where += " " + condition + " And";
                                }
                            }

                            where = where.Substring(0, where.Length - 3);
                            Query += where;
                        
                    }
                    cmd.Connection = Db.Database.Connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = Query;

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

        public void WhereTable(List<TableDetail> tables , List<string> where)
        {
            foreach(var table in tables)
            {
                var main = (TableDescription)TableHashTableHelper.tableTable[table.TableName];
                foreach(var name in tables)
                {
                    var key = (TableDescription)TableHashTableHelper.tableTable[name.TableName];
                    if (main.TableName != key.TableName)
                    {
                        foreach (var pk in main.PrimaryKey)
                        {
                            int index = key.ForeignKey.IndexOf(pk);
                            if (index != -1)
                            {
                                string condition = main.TableName + "." + pk + " = " + key.TableName + "." + key.ForeignKey[index];
                                where.Add(condition);
                            }
                        }
                    }
                }
            }
            
        }

        public string JoiningTable(List<TableDetail> tables)
        {
            string from = "";
            foreach (var table in tables)
            {
                var main = (TableDescription)TableHashTableHelper.tableTable[table.TableName];
                string condition = main.TableName + " ";
                bool isFound = false;
                foreach (var name in tables)
                {
                    var key = (TableDescription)TableHashTableHelper.tableTable[name.TableName];
                    if (main.TableName != key.TableName)
                    {
                        foreach (var fk in main.ForeignKey)
                        {
                            int index = key.PrimaryKey.IndexOf(fk);
                            if (index != -1)
                            {
                                condition += " left outer join " + key.TableName +" on " + main.TableName+"."+fk+" = "+key.TableName+"."+key.PrimaryKey[index]+" ";
                                isFound = true;
                            }
                        }
                    }
                }
                condition += ", ";
 
                if (isFound)
                {
                    from += condition;
                }
            }

            return from.Substring(0,from.Length-2);
        }

        //public DataTable GetReportData(int ReportIDSys)
        //{
        //    //Oil Comment
        //    using (WMSDbContext Db = new WMSDbContext())
        //    {
        //        DbProviderFactory dbFactory = DbProviderFactories.GetFactory(Db.Database.Connection);

        //        using (var cmd = dbFactory.CreateCommand())
        //        {
        //            cmd.Connection = Db.Database.Connection;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "dbo.ProcGetReportData";

        //            DbParameter param = cmd.CreateParameter();
        //            param.ParameterName = "@ReportID";
        //            param.Value = ReportIDSys;

        //            cmd.Parameters.Add(param);
        //            using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
        //            {
        //                adapter.SelectCommand = cmd;

        //                DataTable dt = new DataTable();
        //                adapter.Fill(dt);

        //                return dt;
        //            }
        //        }
        //    }
        //}

    }
}
