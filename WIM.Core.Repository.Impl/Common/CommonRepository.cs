using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.Common;
using WMS.Context;

namespace WIM.Core.Repository.Impl
{
    public class CommonRepository : Repository<UserLog> , ICommonRepository
    {
        private CoreDbContext Db { get; set; }
        public CommonRepository(CoreDbContext context):base(context)
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
            WMSDbContext wms = new WMSDbContext();
            return wms.Database.SqlQuery<string>("ProcGetTableDescription @tableName"
                , new SqlParameter("@tableName", tableName)).FirstOrDefault();
        }

        public IEnumerable<UserLog> ProcGetUserLog(int? logID, string requestMethod, string requestUrl, DateTime? requestDateFrom, DateTime? requestDateTo)
        {
            var logIDParameter = logID.HasValue ?
               new SqlParameter("@LogID", logID) :
               new SqlParameter("@LogID", typeof(int));

            var requestMethodParameter = requestMethod != null ?
                new SqlParameter("@RequestMethod", requestMethod) :
                new SqlParameter("@RequestMethod", typeof(string));

            var requestUrlParameter = requestUrl != null ?
                new SqlParameter("@RequestUrl", requestUrl) :
                new SqlParameter("@RequestUrl", typeof(string));

            var requestDateFromParameter = requestDateFrom.HasValue ?
                new SqlParameter("@RequestDateFrom", requestDateFrom) :
                new SqlParameter("@RequestDateFrom", typeof(System.DateTime));

            var requestDateToParameter = requestDateTo.HasValue ?
                new SqlParameter("@RequestDateTo", requestDateTo) :
                new SqlParameter("@RequestDateTo", typeof(System.DateTime));

            return this.Context.Database.SqlQuery<UserLog>("ProcGetUserLog @LogID,@RequestMethod,@RequestUrl,@RequestDateFrom,@RequestDateTo"
                 , logIDParameter
                 , requestMethodParameter
                 , requestUrlParameter
                 , requestDateFromParameter
                 , requestDateToParameter).ToList();
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
                WMSDbContext wms = new WMSDbContext();
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
