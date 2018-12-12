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
        //Handy
        ItemImportDto GetItemByDocID_Handy(string id);
        void DeleteItem(string id);
        int SetSerial2Box(string boxNumberFrom, string boxNumberTo, ItemGroupRequest ItemGroup, string emID);
        int SetBox2Location(string locationTo, ItemGroupRequest boxList, string emID);
        bool Receive(ReceiveRequest receive);
        List<string> GetItemGroupByOrderNo_Handy(string orderNo);
        bool ConfirmPicking(ConfirmPickingRequest confirmRequest);
        bool SetScanned(SetScannedRequest receive);
        bool RegisterRFID_HANDY(RegisterRFIDRequest registerRequest);

        //Default
        IEnumerable<ImportSerialHead> GetItems();
        IEnumerable<ImportSerialHead> GetItems(int pageIndex, int pageSize,out int totalRecord);
        IEnumerable<FujiPickingGroup> GetPickingGroup(int max = 50);
        IEnumerable<ImportSerialHead> GetDataByColumn(ParameterSearch parameterSearch, out int totalRecord);
        ImportSerialHead GetItemByDocID(string id, bool isIncludeChild = false);
        string GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword);
        ImportSerialHead CreateItem(ImportSerialHead Item);
        bool UpdateItem(string id, ImportSerialHead Item);
        Task<bool> UpdateItemAsync(string id, ImportSerialHead item);
        IEnumerable<ImportSerialDetail> UpdateStatus(List<PickingRequest> pickingList);
        bool UpdateStausExport(ImportSerialHead item);
        bool ClearPickingGroup(string orderID);
        FujiPickingGroup GetPickingByOrderNo(string orderNo, bool isAddItem = false);
        IEnumerable<ImportSerialDetail> FindImportSerialDetailByCriteria(ParameterSearch parameterSearch, out int totalRecord);
        IEnumerable<FujiBoxNumberAndAmountModel> GetBoxNumberAndAmountList(ParameterSearch parameterSearch);
        IEnumerable<FujiSerialAndRFIDModel> GetItemsInBoxNumber(string boxNumber);
        FujiCheckRegister GetLastestBoxNumberItems();
        FujiCheckRegister GetItemScanLastest();
        IEnumerable<ImportSerialHead> GetHeadDataTopten(ParameterSearch parameterSearch);
        string GetRFIDInfo(ParameterSearch parameter);
        IEnumerable<FujiTagReport> GetReportByYearRang(ParameterSearch parameterSearch);
        StreamContent GetReportStream(ImportSerialHead item);

        //Async


    }
}
