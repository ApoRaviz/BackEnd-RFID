using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ImportManagement;
using WIM.Core.Service;

namespace WIM.Core.Service.Import
{
    public interface IImportMasterService : IService
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
