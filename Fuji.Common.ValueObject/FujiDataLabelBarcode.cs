using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiDataLabelBarcode
    {
        public byte[] Barcode { get; private set; }
        public string BarcodeInfo { get; private set; }
        public FujiDataLabelBarcode(byte[] barcode, string barcodeInfo)
        {
            this.Barcode = barcode;
            this.BarcodeInfo = barcodeInfo;
        }
    }
}
