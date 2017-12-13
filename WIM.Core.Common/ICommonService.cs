using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.CustomerManagement;

namespace WIM.Core.Common
{
    public interface ICommonService
    {
        string GetTableDescription(String tableName);
        string GetTableDescriptionWms(String tableName);
        bool WriteUserLog(UserLog log);
        IList<UserLog> UserLogData();
        IList<ProcGetUserLog_Result> GetUserLogData(int? logId, DateTime? RequestDateFrom, DateTime? RequestDateTo);
        IList<ProcGetUserLog_Result> GetUserLogData(string RequestMethod, string RequestUrl, DateTime? RequestDateFrom, DateTime? RequestDateTo);
        IList<ProcGetTableColumnsDescription_Result> GetTableColumnsDescription(string TableName);
        string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword);
        T AutoMapper<T>(object data);
    }
}
