using System;
using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Common;
using WMS.Common.ValueObject;

namespace WIM.Core.Repository
{
    public interface ICommonRepository : IRepository<UserLog>
    {
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
        string ProcGetTableDescription(string tableName);
        string ProcGetTableDescriptionWms(string tableName);
        IEnumerable<UserLog> ProcGetUserLog(Nullable<int> logID, string requestMethod, string requestUrl, string requestUrlFrontEnd, string requestMenuNameFrontEnd, Nullable<System.DateTime> requestDateFrom, Nullable<System.DateTime> requestDateTo);
        IEnumerable<TableColumnsDescription> ProcGetTableColumnsDescription(string tableName);
        string ProcGetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword);
        
    }
}
