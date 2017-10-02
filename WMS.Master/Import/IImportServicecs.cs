﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master.Import
{
    public interface IImportService
    {
        List<ImportDefinitionHeader_MT> GetAllImportHeader(string forTable);
        ImportDefinitionHeader_MT GetImportDefinitionByImportIDSys(int id, string include);
        int? CreateImportDifinitionForItemMaster(ImportDefinitionHeader_MT data);
        bool UpdateImportForItemMaster(int ImportIDSys, ImportDefinitionHeader_MT data);
        string ImportDataToTable(int ImportIDSys, string data);
        void InsertImportHistory(int ImportIDSys, string fileName, string result, bool success, string user);
        bool DeleteImport(int ImportIDSys);
    }
}
