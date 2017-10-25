using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiDataBarcode
    {
        public byte[] Barcode { get; private set; }
        public string BarcodeInfo { get; private set; }
        public string WarehouseCode { get; private set; }
        public string ItemCode { get; private set; }
        public string InvoiceNumber { get; private set; }
        public string LotNumber { get; private set; }
        public string ReceivingDate { get; private set; }
        public string Qty { get; private set; }
        public string Location { get; private set; }

        public FujiDataBarcode(byte[] barcode, string barcodeInfo, string warehouseCode, string itemCode, string invoiceNumber, string lotNumber, string receivingDate, string qty, string location)
        {
            this.Barcode = barcode;
            this.BarcodeInfo = barcodeInfo;
            this.WarehouseCode = warehouseCode;
            this.ItemCode = itemCode;
            this.InvoiceNumber = invoiceNumber;
            this.LotNumber = lotNumber;
            this.ReceivingDate = receivingDate;
            this.Qty = qty;
            this.Location = location;
        }
    }
}
