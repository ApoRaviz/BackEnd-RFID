using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Entity.ImportManagement;

namespace WMS.Service.Import
{
    public interface IImportService : IService
    {
        List<ImportDefinitionHeader_MT> GetAllImportHeader(int projectId, string forTable);
        ImportDefinitionHeader_MT GetImportDefinitionByImportIDSys(int id, string include);
        List<ImportHistory> GetImportHistoryByImportIDSys(int id,int projectId);
        int? CreateImportDifinitionForItemMaster(ImportDefinitionHeader_MT data);
        bool UpdateImportForItemMaster(int ImportIDSys, ImportDefinitionHeader_MT data);
        string ImportDataToTable(int ImportIDSys, string data,string userUpdate);
        void InsertImportHistory(int ImportIDSys, string fileName, string result, bool success, string user);
        bool DeleteImport(int ImportIDSys);
    }
}
