using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiDataBarcodeDetail
    {
        public string ItemCode { get; private set; }
        public string SerialNumber { get; private set; }
        public string BoxNumber { get; private set; }
        public string ItemGroup { get; private set; }
        public FujiDataBarcodeDetail(string itemCode, string serialNumber, string boxNumber, string itemGroup)
        {
            this.ItemCode = itemCode;
            this.SerialNumber = serialNumber;
            this.BoxNumber = boxNumber;
            this.ItemGroup = itemGroup;
        }
    }
}
