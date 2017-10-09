using Isuzu.Common.ValueObject;
using Isuzu.Repository;
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
        bool CheckScanRepeatRegisterInboundItem_HANDY(InboundItemHandyDto inboundItem);
        void RegisterInboundItem_HANDY(InboundItemHandyDto inboundItem, string username);
        int GetAmountInboundItemInInvoiceByRFID_HANDY(string rfid);
        InboundItemHandyDto GetInboundItemByRFID_HANDY(string rfid);
        IEnumerable<InboundItemHandyDto> GetInboundItemsByInvoice_HANDY(string rfid);
        void PerformHolding_HANDY(InboundItemHoldingHandyRequest inboundItemHold, string username);
        void PerformShipping_HANDY(InboundItemShippingHandyRequest inboundItemShipping, string username);
        void PerformPackingCarton_HANDY(InboundItemCartonPackingHandyRequest inboundItemCartonPacking, string username);
        void PerformPackingCase_HANDY(InboundItemCasePackingHandyRequest inboundItemCasePacking, string username);
        InboundItemCartonHandyDto GetInboundItemCartonByRFID_HANDY(string rfid);
        IEnumerable<InboundItems> GetInboundItemsByRFIDs_HANDY(RFIDList rfids);

        //Default
        InboundItems GetInboundItemByISZJOrder(string iszjOrder);
        IEnumerable<InboundItems> GetInboundItemPaging(int pageIndex, int pageSize, out int totalRecord);
        IEnumerable<InboundItems> ImportInboundItemList(List<InboundItems> itemList,string userName);
        IEnumerable<InboundItems> GetInboundItemByQty(int Qty, bool isShipped = false);
        IEnumerable<InboundItems> GetInboundItemByInvoiceNumber(string invNo,bool isShipped = false);
        IEnumerable<InboundItems> GetDataByColumn(ParameterSearch parameterSearch);
        IEnumerable<InboundItemsHead> GetDataGroupByColumn(string column, string keyword);
        IEnumerable<InboundItemsHead> GetInboundGroupPaging(int pageIndex, int pageSize, out int totalRecord);
        IEnumerable<InboundItemsHead> GetInboundGroup(int max = 20);
        InboundItemsHead GetInboundGroupByInvoiceNumber(string invNo);
        bool UpdateStausExport(InboundItemsHead item);
        bool UpdateDeleteReason(IsuzuDeleteReason reason);
        bool UpdateQtyInboundHead(string invNo);
    }
}
