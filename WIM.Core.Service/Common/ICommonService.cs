﻿using System;
using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Common;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Service;
using WIM.Common.ValueObject;

namespace WIM.Core.Service
{
    public interface ICommonService : IService
    {
        string GetTableDescription(String tableName);
        string GetTableDescriptionWms(String tableName);
        bool WriteUserLog(UserLog log);
        IEnumerable<UserLog> UserLogData();
        IEnumerable<UserLog> GetUserLogData(int? logId, DateTime? RequestDateFrom, DateTime? RequestDateTo);
        IEnumerable<UserLog> GetUserLogData(string RequestMethod, string RequestUrl, string RequestUrlFrontEnd, string RequestMenuNameFrontEnd, DateTime? RequestDateFrom, DateTime? RequestDateTo);
        //IEnumerable<UserLog> GetUserLogData2(LogMasterParameters logMasterParameters);
        //IEnumerable<UserLogDto> GetUserLogData3(LogMasterParameters logMasterParameters);
        IEnumerable<TableColumnsDescription> GetTableColumnsDescription(string TableName);
        string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword);
        IEnumerable<SubModuleDto> SMAutoComplete(string key);
        //void InsertLog(HandheldErrorLog errorLog);
        T AutoMapper<T>(object data);
        string GetValueGenerateCode(string key);
        string GetValidation(List<string> tableName);
    }
}
