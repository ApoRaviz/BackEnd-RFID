using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;

namespace WIM.Core.Common
{
    public class CommonService : ICommonService
    {
        private CommonContext db = CommonContext.Create();

        public string GetTableDescription(String tableName)
        {
            ObjectParameter result = new ObjectParameter("tableName", tableName);
            return db.ProcGetTableDescription(tableName).FirstOrDefault();
        }

        public string GetTableDescriptionWms(String tableName)
        {
            ObjectParameter result = new ObjectParameter("tableName", tableName);
            return db.ProcGetTableDescriptionWms(tableName);
        }

        public bool WriteUserLog(UserLog log)
        {
            return false;

            /*var logRepo = new GenericRepository<UserLog>(db);

            using (var scope = new TransactionScope())
            {
                logRepo.Insert(log);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException)
                {
                    
                }
                scope.Complete();
                return true;
            }*/
        }

        public IList<UserLog> UserLogData()
        {
            return null;
            /*var logRepo = new GenericRepository<UserLog>(db);
            return logRepo.GetAll().ToList();*/
        }

        public IList<ProcGetUserLog_Result> GetUserLogData(int? logId, DateTime? RequestDateFrom, DateTime? RequestDateTo)
        {
            return db.ProcGetUserLog(logId, null, null, RequestDateFrom, RequestDateTo).ToList();
        }

        public IList<ProcGetUserLog_Result> GetUserLogData(string RequestMethod, string RequestUrl, DateTime? RequestDateFrom, DateTime? RequestDateTo)
        {
            return db.ProcGetUserLog(null, RequestMethod, RequestUrl, RequestDateFrom, RequestDateTo).ToList();
        }

        public IList<ProcGetTableColumnsDescription_Result> GetTableColumnsDescription(string TableName)
        {
            return db.ProcGetTableColumnsDescription(TableName);
        }

        public string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword)
        {
            var z = db.ProcGetDataAutoComplete(columnNames, tableName, conditionColumnNames, keyword)/*.FirstOrDefault()*/;
            return z;
        }

        public IEnumerable<SubModuleDto> SMAutoComplete(string key)
        {
            using(CoreDbContext Db = new CoreDbContext())
            {
               return Db.AutoCompleteSM(key);
            }
        }

        public void InsertLog(HandheldErrorLog errorLog)
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
                if(respType.GetProperty(prop.Name) != null) 
                respType.GetProperty(prop.Name).SetValue(resp, value, null);
            }

            return resp;

        }
    }
}
