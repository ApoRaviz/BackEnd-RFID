﻿using System;
using System.Collections.Generic;
using WIM.Core.Entity.importManagement;

namespace WIM.Core.Service
{
    public interface IImportDataService : IService
    {
        List<ImportDefinitionHeader_MT> GetAllImportHeader(string forTable);
        ImportDefinitionHeader_MT GetImportDefinitionByImportIDSys(int id, string include);
        int? CreateImportDifinitionForItemMaster(ImportDefinitionHeader_MT data);
        bool UpdateImportForItemMaster(int ImportIDSys, ImportDefinitionHeader_MT data);
        string ImportDataToTable(int ImportIDSys, string data,string userUpdate);
        void InsertImportHistory(int ImportIDSys, string fileName, string result, bool success, string user);
        bool DeleteImport(int ImportIDSys);
    }
}
