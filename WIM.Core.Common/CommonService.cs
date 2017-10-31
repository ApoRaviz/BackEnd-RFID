﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

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
            return db.ProcGetTableColumnsDescription(TableName).ToList();
        }

        public string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword)
        {
            return db.ProcGetDataAutoComplete(columnNames, tableName, conditionColumnNames, keyword).FirstOrDefault();
        }

        public void InsertLog(HandheldErrorLog errorLog)
        {

        }
    }
}
