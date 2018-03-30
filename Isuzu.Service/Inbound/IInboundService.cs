using Isuzu.Common.ValueObject;
using Isuzu.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Service
{
    public interface IInboundService
    {
        //Handy
        InboundItemHandyDto GetInboundItemByISZJOrder_HANDY(string iszjOrder);
        int GetAmountRegistered_HANDY();
        int GetAmountInboundItemInInvoiceByRFID_HANDY(string rfid);
        InboundItemHandyDto GetInboundItemByRFID_HANDY(string rfid);
        IEnumerable<InboundItemHandyDto> GetInboundItemsByInvoice_HANDY(string rfid);
        bool CheckScanRepeatRegisterInboundItem_HANDY(InboundItemHandyDto inboundItem);
        void RegisterInboundItem_HANDY(InboundItemHandyDto item);
        void PerformHolding_HANDY(InboundItemHoldingHandyRequest itemsHolding);
        void PerformShipping_HANDY(InboundItemShippingHandyRequest itemsShipping);
        void PerformPackingCarton_HANDY(InboundItemCartonPackingHandyRequest inboundItemCartonPacking);
        void PerformPackingCase_HANDY(InboundItemCasePackingHandyRequest inboundItemCasePacking);
        InboundItemCartonHandyDto GetInboundItemCartonByRFID_HANDY(string rfid);
        IEnumerable<InboundItems> GetInboundItemsByRFIDs_HANDY(RFIDList rfids);

        //Default
        InboundItems GetInboundItemByISZJOrder(string iszjOrder);
        IEnumerable<InboundItems> GetInboundItemPaging(int pageIndex, int pageSize, out int totalRecord);
        IEnumerable<InboundItems> GetInboundItemDeletedPaging(int pageIndex, int pageSize, out int totalRecord);
        List<InboundItems> ImportInboundItemList(List<InboundItems> itemList);
        IEnumerable<InboundItems> GetInboundItemByQty(int Qty, bool isShipped = false);
        IEnumerable<InboundItems> GetInboundItemByInvoiceNumber(string invNo,bool isShipped = false);
        IEnumerable<InboundItems> GetDataByColumn(ParameterSearch parameterSearch);
        IEnumerable<InboundItemsHead> GetDataGroupByColumn(string column, string keyword);
        IEnumerable<InboundItemsHead> GetInboundGroupPaging(int pageIndex, int pageSize, out int totalRecord);
        IEnumerable<InboundItemsHead> GetInboundGroup(int max = 20);
        InboundItemsHead GetInboundGroupByInvoiceNumber(string invNo,bool isAddItems = false);
        bool UpdateStausExport(InboundItemsHead item);
        bool UpdateDeleteReason(IsuzuDeleteReason reason);
        bool UpdateDeleteReasonByInvoice(string InvNo, IsuzuDeleteReason reason);
        bool UpdateQtyInboundHead(string invNo,string userUpdate);
        IsuzuDataImport OpenReadExcel(string localFileName);
        string GetRFIDInfo(ParameterSearch parameter);
        IEnumerable<IsuzuTagReport> GetReportByYearRang(ParameterSearch parameterSearch);
        string CreateDeletedFileID(string pathName);
        void GetDeletedFileID(string fileID);
        AdjustWeight GetBeforeAdjustWeight(AdjustWeight adjustWeight);
        void AdjustWeight(AdjustWeight adjustWeight);
        //Async

    }
}
