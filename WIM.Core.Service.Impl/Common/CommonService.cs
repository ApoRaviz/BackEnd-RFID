using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Transactions;
using System.Data.Entity.Infrastructure;
using WIM.Core.Context;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.ValueObject;
using System;
using System.Reflection;
using System.Data.SqlClient;
using System.Linq;
using WIM.Core.Common;
using WIM.Core.Entity.Common;
using WIM.Core.Repository.Impl;
using WIM.Core.Repository;
using WIM.Core.Repository.Common;
using WIM.Core.Repository.Impl.Common;
using System.Data.Linq.SqlClient;

namespace WIM.Core.Service.Impl
{
    public class CommonService : WIM.Core.Service.Impl.Service, ICommonService
    {
        public CommonService()
        {
        }

        public T AutoMapper<T>(object data)
        {
            Type typeEntityToUpdate = data.GetType();
            PropertyInfo[] properties = typeEntityToUpdate.GetProperties();
            T resp = Activator.CreateInstance<T>();
            Type respType = resp.GetType();
            foreach (PropertyInfo prop in properties)
            {
                var value = prop.GetValue(data);
                if (respType.GetProperty(prop.Name) != null)
                {
                    try
                    {
                        respType.GetProperty(prop.Name).SetValue(resp, value, null);
                    }
                    catch (Exception)
                    {
                    }

                }

            }

            return resp;
        }

        public string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword)
        {
            string tableDescription = "";
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                tableDescription = repo.ProcGetDataAutoComplete(columnNames, tableName, conditionColumnNames, keyword);
            }
            return tableDescription;
        }

        public string GetTableDescription(string tableName)
        {
            string tableDescription = "";
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                tableDescription = repo.ProcGetTableDescription(tableName);
            }
            return tableDescription;
        }

        public string GetTableDescriptionWms(string tableName)
        {
            string tableDescriptionWms = "";
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                tableDescriptionWms = repo.ProcGetTableDescriptionWms(tableName);
            }
            return tableDescriptionWms;
        }

        public string GetValidation(string tableName)
        {
            string validation = "";
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                validation = repo.GetValidation(tableName);
            }
            return validation;
        }



        public IEnumerable<UserLog> GetUserLogData(int? logId, DateTime? RequestDateFrom, DateTime? RequestDateTo)
        {
            IEnumerable<UserLog> userLogs;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                userLogs = repo.ProcGetUserLog(logId, null, null, null, RequestDateFrom, RequestDateTo);

                /*UserLog userLog = userLogs.FirstOrDefault(x => x.LogID == 177053);
                string reqUrl = StringHelper.GetRequestUrl(userLog.RequestUri);
                string[] reqUrlSplit = reqUrl.Split('/');
                if (reqUrlSplit.Length > 3)
                {

                }*/
            }
            return userLogs;
        }

        public IEnumerable<UserLog> GetUserLogData(string RequestMethod, string RequestUrl, string RequestUrlFrontEnd, DateTime? RequestDateFrom, DateTime? RequestDateTo)
        {
            IEnumerable<UserLog> userLogs;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                userLogs = repo.ProcGetUserLog(null, RequestMethod, RequestUrl, RequestUrlFrontEnd, RequestDateFrom, RequestDateTo);
            }
            return userLogs;
        }

        public void InsertLog(HandheldErrorLog errorLog)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubModuleDto> SMAutoComplete(string key)
        {
            using (CoreDbContext Db = new CoreDbContext())
            {
                return Db.AutoCompleteSM(key);
            }
        }

        public IEnumerable<UserLog> UserLogData()
        {
            IEnumerable<UserLog> userLog;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                userLog = repo.Get();
            }
            return userLog;
        }

        public bool WriteUserLog(UserLog log)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ICommonRepository repo = new CommonRepository(Db);
                        repo.Insert(log);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
            }
            return true;
        }

        public IEnumerable<TableColumnsDescription> GetTableColumnsDescription(string tableName)
        {
            IEnumerable<TableColumnsDescription> tableColumnsDescription;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                tableColumnsDescription = repo.ProcGetTableColumnsDescription(tableName);
            }
            return tableColumnsDescription;
        }

        public string GetValueGenerateCode(string key)
        {
            string KeyValue = "";
            using (CoreDbContext Db = new CoreDbContext())
            {
                IGeneralConfigsRepository repo = new GeneralConfigsRepository(Db);
                var value = repo.GetByID(key);
                DetailConfig temp = new DetailConfig();
                temp.Value = value.DetailConfig.Value;
                temp.IsReset = value.DetailConfig.IsReset;
                temp.Key = value.DetailConfig.Key;
                if (value != null)
                {
                    if (value.DetailConfig.IsReset == "D")
                    {
                        if (value.UpdateAt.Value.ToString("d") != DateTime.Now.ToString("d"))
                        {
                            temp.Key = 0.ToString();
                        }
                    }
                    else if (value.DetailConfig.IsReset == "M")
                    {
                        if (value.UpdateAt.Value.ToString("M") != DateTime.Now.ToString("M"))
                        {
                            temp.Key = 0.ToString();
                        }
                    }
                    else if (value.DetailConfig.IsReset == "Y")
                    {
                        if (value.UpdateAt.Value.ToString("yyyy") != DateTime.Now.ToString("yyyy"))
                        {
                            temp.Key = 0.ToString();
                        }
                    }
                    temp.Key = (int.Parse(temp.Key) + 1).ToString();
                    var newCode = GenerateNewCode(temp);
                    value.DetailConfig = temp;
                    KeyValue = newCode;
                    repo.Update(value);
                    Db.SaveChanges();
                }
            }
            return KeyValue;
        }

        private string GenerateNewCode(DetailConfig value)
        {
            List<string> Cases = new List<string>();
            string alpha = value.Value.Substring(0, 1);
            Cases = value.Value.Split('|').ToList();

            string[] TimeFormat = { "yy", "yyyy", "mm", "dd", "d", "h", "hh", "HH", "H", "M", "MM", "s", "ss", "t", "tt" };
            string GenerateCode = "";


            foreach (var a in Cases)
            {
                if (TimeFormat.Contains(a))
                {
                    GenerateCode += DateTime.Now.ToString(a);
                }
                else if (a.Contains("#"))
                {
                    var lastnumber = int.Parse(value.Key);
                    GenerateCode += lastnumber.ToString("D" + a.Length);
                }
                else
                {
                    GenerateCode += a;
                }
            }
            return GenerateCode;
        }

        public bool CreateValueGenerateCode(GeneralConfigs value)
        {
            using (var scope = new TransactionScope())
            {
                using (CoreDbContext Db = new CoreDbContext())
                {
                    IGeneralConfigsRepository repo = new GeneralConfigsRepository(Db);
                    try
                    {
                        //GeneralConfigs general = new GeneralConfigs();
                        //general.DetailConfig = value;
                        //general.Keyword = "CheckStock";
                        repo.Insert(value);
                        scope.Complete();

                    }
                    catch (DbEntityValidationException e)
                    {
                        scope.Dispose();
                        return false;
                        throw new AppValidationException(e);
                    }
                }
            }
            return true;

        }
        //for(int i = 1; i < format.Length; i++)
        //{
        //    if(format.Substring(i,1) == alpha)
        //    {
        //        alpha += format.Substring(i, 1);
        //        if(i == format.Length)
        //        {
        //            Cases.Add(alpha);
        //        }
        //    }
        //    else
        //    {
        //        Cases.Add(alpha);
        //        alpha = format.Substring(i, 1);
        //    }
        //}
    }
}
