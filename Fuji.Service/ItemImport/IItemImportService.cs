using Fuji.Common.ValueObject;
using Fuji.Entity.ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Service.ItemImport
{
    public interface IItemImportService
    {
        IEnumerable<ImportSerialHead> GetItems();
        IEnumerable<ImportSerialHead> GetItems(int pageIndex, int pageSize,out int totalRecord);
        IEnumerable<FujiPickingGroup> GetPickingGroup(int max = 50);
        IEnumerable<ImportSerialDetail> GetImportSerialDetailByHeadID(string headID);
        IEnumerable<ImportSerialHead> GetDataByColumn(string column, string keyword);
        ImportSerialHead GetItemByDocID(string id);
        ItemImportDto GetItemByDocID_Handy(string id);
        string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword);
        ImportSerialHead CreateItem(ImportSerialHead Item);
        bool UpdateItem(string id, ImportSerialHead Item);
        IEnumerable<ImportSerialDetail> UpdateStatus(List<PickingRequest> pickingList, string userUpdate);
        void DeleteItem(string id);
        bool Receive(ReceiveRequest receive);
        List<string> GetItemGroupByOrderNo_Handy(string orderNo);
        bool ConfirmPicking(ConfirmPickingRequest confirmRequest, string userUpdate);
        bool SetScanned(SetScannedRequest receive);
        bool UpdateStausExport(ImportSerialHead item);
        bool ClearPickingGroup(string orderID);
        FujiPickingGroup GetPickingByOrderNo(string orderNo, bool isAddItem = false);
        bool RegisterRFID_HANDY(RegisterRFIDRequest registerRequest, string username);
        IEnumerable<ImportSerialDetail> FindImportSerialDetailByCriteria(ParameterSearch parameterSearch);
    }
}
