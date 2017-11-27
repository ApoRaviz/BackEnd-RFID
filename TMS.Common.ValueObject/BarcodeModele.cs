using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ValueObject
{
    public class BarcodeModele
    {
            public byte[] Barcode { get; private set; }
            public string BarcodeInfo { get; private set; }
            public BarcodeModele(byte[] barcode, string barcodeInfo)
            {
                Barcode = barcode;
                BarcodeInfo = barcodeInfo;
            }
        
    }
}
