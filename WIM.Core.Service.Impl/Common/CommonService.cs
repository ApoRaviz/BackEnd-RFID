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
                    catch(Exception e)
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

        public IEnumerable<UserLog> GetUserLogData(int? logId, DateTime? RequestDateFrom, DateTime? RequestDateTo)
        {
            IEnumerable<UserLog> userLogs;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                userLogs = repo.ProcGetUserLog(logId,null,null, RequestDateFrom, RequestDateTo);
            }
            return userLogs;
        }

        public IEnumerable<UserLog> GetUserLogData(string RequestMethod, string RequestUrl, DateTime? RequestDateFrom, DateTime? RequestDateTo)
        {
            IEnumerable<UserLog> userLogs;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                userLogs = repo.ProcGetUserLog(null, RequestMethod, RequestUrl, RequestDateFrom, RequestDateTo);
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
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.E4012);
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
    }
}
