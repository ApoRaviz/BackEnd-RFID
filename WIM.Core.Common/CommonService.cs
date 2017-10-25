using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using System.Transactions;
using WIM.Core.Common.Validation;
using System.Data.Entity.Validation;
using WIM.Core.Repository.Impl;

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
            var logRepo = new GenericRepository<UserLog>(db);

            using (var scope = new TransactionScope())
            {
                //logRepo.Insert(log);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException)
                {
                    
                }
                scope.Complete();
                return true;
            }
        }

        public IList<UserLog> UserLogData()
        {
            var logRepo = new GenericRepository<UserLog>(db);
            //return logRepo.GetAll().ToList();
            return null;
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
