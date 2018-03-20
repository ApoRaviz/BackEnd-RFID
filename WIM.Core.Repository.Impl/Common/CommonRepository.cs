using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.Common;
namespace WIM.Core.Repository.Impl
{
    public class CommonRepository : Repository<UserLog> , ICommonRepository
    {
        private CoreDbContext Db { get; set; }
        public CommonRepository(Context.CoreDbContext context):base(context)
        {
            Db = context;
        }

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return this.Context.Database.SqlQuery<T>(sql, parameters);
        }

        public string ProcGetTableDescription(string tableName)
        {
           return this.Context.Database.SqlQuery<string>("ProcGetTableDescription @tableName"
                , new SqlParameter("@tableName", tableName)).FirstOrDefault();
        }

        public string ProcGetTableDescriptionWms(string tableName)
        {
            CoreDbContext wms = new CoreDbContext();
            return wms.Database.SqlQuery<string>("ProcGetTableDescription @tableName"
                , new SqlParameter("@tableName", tableName)).FirstOrDefault();
        }

        public IEnumerable<UserLog> ProcGetUserLog(int? logID, string requestMethod, string requestUrl, DateTime? requestDateFrom, DateTime? requestDateTo)
        {

            var logIDParameter = logID.HasValue ? new SqlParameter
            {
                ParameterName = "@LogID",
                Value = logID
            } : new SqlParameter() {
                ParameterName = "@LogID",
                Value = 0,
                DbType = DbType.Int32
            };

            var requestMethodParameter = requestMethod != null ? new SqlParameter
            {
                ParameterName = "@RequestMethod",
                Value = logID
            } : new SqlParameter("@RequestMethod", DBNull.Value);

            var requestUrlParameter = requestUrl != null ? new SqlParameter
            {
                ParameterName = "@RequestUrl",
                Value = requestUrl
            } : new SqlParameter("@RequestUrl", DBNull.Value);

            var requestDateFromParameter = requestDateFrom.HasValue ? new SqlParameter
            {
                ParameterName = "@RequestDateFrom",
                Value = requestDateFrom
            } : new SqlParameter("@RequestDateFrom", typeof(System.DateTime));

            var requestDateToParameter = requestDateTo.HasValue ? new SqlParameter
            {
                ParameterName = "@RequestDateTo",
                Value = requestDateFrom
            } : new SqlParameter("@RequestDateTo", typeof(System.DateTime));

            return this.Context.Database.SqlQuery<UserLog>("exec ProcGetUserLog @LogID , @RequestMethod , @RequestUrl , @RequestDateFrom ," +
            "@RequestDateTo", logIDParameter, requestMethodParameter, requestUrlParameter, requestDateFromParameter, requestDateToParameter).ToList();
        }

        public IEnumerable<TableColumnsDescription> ProcGetTableColumnsDescription(string tableName)
        {
           var tableNameParameter = /*new ObjectParameter("@tableName", tableName);*/
           new SqlParameter("tableName", tableName);

            List<TableColumnsDescription>  x = this.Context.Database.SqlQuery<TableColumnsDescription>("ProcGetTableColumnsDescription @tableName", tableNameParameter).ToList();

            if (x.Count == 0)
            {
                var tableNameParameter2 = /*new ObjectParameter("@tableName", tableName);*/
                    new SqlParameter("tableName", tableName);
                //#OilComment
                CoreDbContext wms = new CoreDbContext();
                x = wms.Database.SqlQuery<TableColumnsDescription>("ProcGetTableColumnsDescription @tableName", tableNameParameter2).ToList();
            }
            return x;
        }

        public string ProcGetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword)
        {
            var columnNamesParameter = /*new ObjectParameter("@columnNames", columnNames);*/
            new SqlParameter("@columnNames", columnNames);

            var tableNameParameter = /*new ObjectParameter("@tableName", tableName);*/
            new SqlParameter("@tableName", tableName);

            var conditionColumnNamesParameter = /*new ObjectParameter("@conditionColumnNames", conditionColumnNames);*/
            new SqlParameter("@conditionColumnNames", conditionColumnNames);

            var keywordParameter = /*new ObjectParameter("@keyword", keyword);*/
            new SqlParameter("@keyword", keyword);
            string x;

            x = this.Context.Database.SqlQuery<string>("ProcGetDataAutoComplete @columnNames, @tableName, @conditionColumnNames, @keyword", columnNamesParameter, tableNameParameter, conditionColumnNamesParameter, keywordParameter).FirstOrDefault();


            return x;
        }

    }
}
