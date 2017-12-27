using Fuji.Common.ValueObject;
using Fuji.Entity.ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;

namespace Fuji.Service.ItemImport
{
    public interface IItemImportService: IService
    {
        IEnumerable<ImportSerialHead> GetItems();
        IEnumerable<ImportSerialHead> GetItems(int pageIndex, int pageSize,out int totalRecord);
        IEnumerable<FujiPickingGroup> GetPickingGroup(int max = 50);
        IEnumerable<ImportSerialDetail> GetImportSerialDetailByHeadID(string headID);
        IEnumerable<ImportSerialHead> GetDataByColumn(ParameterSearch parameterSearch, out int totalRecord);
        ImportSerialHead GetItemByDocID(string id, bool isIncludeChild = false);
        ItemImportDto GetItemByDocID_Handy(string id);
        string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword);
        ImportSerialHead CreateItem(ImportSerialHead Item);
        bool UpdateItem(string id, ImportSerialHead Item);
        IEnumerable<ImportSerialDetail> UpdateStatus(List<PickingRequest> pickingList);
        void DeleteItem(string id);
        bool Receive(ReceiveRequest receive);
        List<string> GetItemGroupByOrderNo_Handy(string orderNo);
        bool ConfirmPicking(ConfirmPickingRequest confirmRequest);
        bool SetScanned(SetScannedRequest receive);
        bool UpdateStausExport(ImportSerialHead item);
        bool ClearPickingGroup(string orderID);
        FujiPickingGroup GetPickingByOrderNo(string orderNo, bool isAddItem = false);
        bool RegisterRFID_HANDY(RegisterRFIDRequest registerRequest);
        IEnumerable<ImportSerialDetail> FindImportSerialDetailByCriteria(ParameterSearch parameterSearch, out int totalRecord);
        StreamContent GetReportStream(ImportSerialHead item);
        IEnumerable<FujiBoxNumberAndAmountModel> GetBoxNumberAndAmountList(ParameterSearch parameterSearch);
        IEnumerable<FujiSerialAndRFIDModel> GetItemsInBoxNumber(string boxNumber);
        FujiCheckRegister GetLastestBoxNumberItems();
        IEnumerable<ImportSerialHead> GetHeadDataTopten(ParameterSearch parameterSearch, out int totalRecord);
        string GetRFIDInfo(ParameterSearch parameter);
        int SetSerial2Box(string boxNumberFrom, string boxNumberTo, ItemGroupRequest ItemGroup, string emID);
        int SetBox2Location(string locationTo, ItemGroupRequest boxList, string emID);
    }
}
