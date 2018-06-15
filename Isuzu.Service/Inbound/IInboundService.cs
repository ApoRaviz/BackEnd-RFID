﻿using Isuzu.Common.ValueObject;
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
        List<RegisterRemaining> GetAmountNewStatusRemaining_HANDY(string invoice);
        List<InboundItemHandyDto> GetUnregisteredOrder_HANDY(string invoice);
        InboundItemHandyDto GetInboundItemByRFID_HANDY(string rfid);
        IEnumerable<InboundItemHandyDto> GetInboundItemsByInvoice_HANDY(string invNo);
        IEnumerable<InboundItemHandyDto> GetInboundItemsRegisteredByInvoice_HANDY(string invNo);
        bool CheckScanRepeatRegisterInboundItem_HANDY(InboundItemHandyDto inboundItem);
        int RegisterInboundItem_HANDY(InboundItemHandyDto item);
        InboundItemHandyDto RegisterInboundItemByOrder_HANDY(InboundItemHandyDto item);
        void PerformHolding_HANDY(List<ConfirmReceiveParameter> itemsHolding);
        void PerformShipping_HANDY(InboundItemShippingHandyRequest itemsShipping);
        void PerformPackingCarton_HANDY(InboundItemCartonPackingHandyRequest inboundItemCartonPacking);
        void UpdateHead_HANDY2(string invNo, string status);

        void PerformPackingCartonNew_HANDY(InboundItemCartonPackingHandyRequestNew inboundItemCartonPacking);
        InboundItemCartonPackingHandyRequestNew GetItemCartonByISZJOrder_HANDY(string ISZJOrder);
        List<InboundItemCartonPackingHandyRequestNew> GetCartonPackedItemByRFID_HANDY(string rfid);

        void PerformPackingCase_HANDY(InboundItemCasePackingHandyRequest inboundItemCasePacking);
        InboundItemCartonHandyDto GetInboundItemCartonByRFID_HANDY(string rfid);
        IEnumerable<InboundItems> GetInboundItemsByRFIDs_HANDY(RFIDList rfids);
        void InsertRFIDTagNotFoundLog(IEnumerable<InboundItems> inboundItems, string functionName);

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
        InboundItemHandyDto GetBeforeAdjustWeight(InboundItemHandyDto adjustWeight);
        void AdjustWeight(InboundItemHandyDto adjustWeight);
        //Async

    }
}
