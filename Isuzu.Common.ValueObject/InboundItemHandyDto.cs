﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class InboundItemHandyDto
    {
        public string ID { get; set; }
        public string InvNo { get; set; }
        public string ITAOrder { get; set; }
        public string RFIDTag { get; set; }
        public string ISZJOrder { get; set; }
        public string Status { get; set; }
        public string RegisterLocation { get; set; }
        public string ReceiveLocation { get; set; }
        public decimal Weight1 { get; set; }
        public decimal Weight2 { get; set; }
        public decimal Weight3 { get; set; }
        public decimal Weight4 { get; set; }
        public decimal Weight5 { get; set; }
        public string PartNo { get; set; }
        public string ParrtName { get; set; }
        public string Vendor { get; set; }
        public int? Qty { get; set; }

        // ForHandheld
        public int IsRepeat { get; set; }
        public int? WeightCursor { get; set; }
    }

    public class InboundItemCartonHandyDto
    {
        public string InvNo { get; set; }
        public string CartonNo { get; set; }
        public string RFIDTag { get; set; }
    }

    public class ReceiveParamsList
    {
        public List<ConfirmReceiveParameter> ReceiveParams { get; set; }
    }

    public class ConfirmReceiveParameter
    {
        public string InvNo { get; set; }
        public string ISZJOrder { get; set; }
        public string RFIDTag { get; set; }
        public Int16 IsFound { get; set; }
    }


    public class InboundItemByPartNumberRequest
    {
        public string InvNo { get; set; }
        public string PartNumber { get; set; }
    }

        public class InboundItemShippingHandyRequest
    {
        public string InvNo { get; set; }
        public List<string> RFIDTags { get; set; }
    }

    public class InboundItemCartonPackingHandyRequest
    {
        public string CartonNo { get; set; }
        public string RFIDTag { get; set; }
    }

    public class InboundItemCartonPackingHandyRequestNew
    {
        public string CartonNo { get; set; }
        public string CaseNo { get; set; }
        public string RFIDTag { get; set; }
        public string ISZJOrder { get; set; }
        public string InvNo { get; set; }
        public List<string> OrderScannedList { get; set; }
        public string function { get; set; }
    }

    public class InboundItemCasePackingHandyRequest
    {
        public string InvNo { get; set; }
        public string CaseNo { get; set; }
        public List<string> RFIDTags { get; set; }
    }

    public class RFIDList
    {        
        public string FunctionName { get; set; }
        public List<string> RFIDTags { get; set; }
    }

    public class RegisterRemaining
    {
        public string InvNo { get; set; }
        public int Qty { get; set; }
        public int Registered { get; set; }
        public int Remaining { get; set; }
    }

    public class InvoiceReportDetail
    {
        public string InvNo { get; set; }
        public string Status { get; set; }
        public int QtyOrder { get; set; }
        public int QtyItem { get; set; }
        public DateTime? RegisterStart { get; set; }
        public DateTime? RegisterEnd { get; set; }
        public DateTime? CartonStart { get; set; }
        public DateTime? CartonEnd { get; set; }
        public DateTime? CaseStart { get; set; }
        public DateTime? CaseEnd { get; set; }
        public DateTime? ShipStart { get; set; }
        public DateTime? ShipEnd { get; set; }
        public int totalCarton { get; set; }
        public int totalCase { get; set; }
    }
    
    public class InvHistoryFilter
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string status { get; set; }
    }
}
