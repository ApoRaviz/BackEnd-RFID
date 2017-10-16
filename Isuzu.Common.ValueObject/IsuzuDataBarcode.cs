using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuDataBarcode
    {
        public byte[] Barcode { get; private set; }
        public string BarcodeInfo { get; private set; }
        public IsuzuDataBarcode(byte[] barcode, string barcodeInfo)
        {
            this.Barcode = barcode;
            this.BarcodeInfo = barcodeInfo;
        }
    }
}
