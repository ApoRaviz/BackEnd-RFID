﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.ExternallInterface.Isuzu.Dtos
{
    public class InboundItemHandyDto
    {
        public string ID { get; set; }
        public string InvNo { get; set; }
        public string ITAOrder { get; set; }
        public string RFIDTag { get; set; }
        public string ISZJOrder { get; set; }

        public int IsRepeat { get; set; }
    }

    public class InboundItemCartonHandyDto
    {
        public string InvNo { get; set; }
        public string CartonNo { get; set; }
        public string RFIDTag { get; set; }

    }

    public class InboundItemHoldingHandyRequest
    {
        public string InvNo { get; set; }
        public List<string> RFIDTags { get; set; }
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

    public class InboundItemCasePackingHandyRequest
    {
        public string InvNo { get; set; }
        public string CaseNo { get; set; }
        public List<string> RFIDTags { get; set; }
    }

    public class RFIDList
    {
        public List<string> RFIDTags { get; set; }
    }
}
