using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Entity.ItemManagement
{
    public class InterfaceReceiveCommand
    {
        public InterfaceReceiveCommand(
            string receiveType, string receiveQualityType
            , string receiveNo, string poNo, string remark
            , DateTime? poDate
            , DateTime? receiveDate, string supplierCode, List<InterfaceReceiveItem> receiveItems)
        {
            ReceiveType = receiveType;
            ReceiveQualityType = receiveQualityType;
            ReceiveNo = receiveNo;
            PoNo = poNo;
            Remark = remark;
            PoDate = poDate;
            ReceiveDate = receiveDate;
            SupplierCode = supplierCode;
            ReceiveItems = receiveItems;
        }

        public string ReceiveType { get; private set; }
        public string ReceiveQualityType { get; private set; }

        public string ReceiveNo { get; private set; }
        public string PoNo { get; private set; }

        public string Remark { get; private set; }

        public DateTime? PoDate { get; private set; }
        public DateTime? ReceiveDate { get; private set; }

        public string SupplierCode { get; private set; }

        public List<InterfaceReceiveItem> ReceiveItems { get; private set; }
    }

    public class InterfaceReceiveItem
    {
        public int Id { get; private set; }
        public string ItemCode { get; private set; }
        public string ScanCode { get; private set; }
        public string JAN { get; private set; }

        public decimal? Qty { get; private set; }
        public string Unit { get; private set; }
        public decimal? SmallestQty { get; private set; }
        public string SmallestUnit { get; private set; }

        public decimal? Cost { get; private set; }
        public decimal? Price { get; private set; }
        public string Currency { get; private set; }

        public decimal? Width { get; private set; }
        public decimal? Height { get; private set; }
        public decimal? Length { get; private set; }
        public decimal? Weight { get; private set; }

        public string Location { get; private set; }

        public DateTime? ExpireDate { get; private set; }
        public DateTime? ManufacturingDate { get; private set; }
        public DateTime? BestBeforeDate { get; private set; }

        public string ControlLevel1 { get; private set; }
        public string ControlLevel2 { get; private set; }
        public string ControlLevel3 { get; private set; }


        public string Remark { get; private set; }

        private readonly List<SerialNumber> _serialNumbers;

        public InterfaceReceiveItem(string itemCode, string scanCode, string jAN
            , decimal? qty, string unit, decimal? smallestQty, string smallestUnit
            , decimal? cost, decimal? price, string currency
            , decimal? width, decimal? height, decimal? length, decimal? weight, string location
            , DateTime? expireDate, DateTime? manufacturingDate, DateTime? bestBeforeDate
            , string controlLevel1, string controlLevel2, string controlLevel3
            , string remark, List<SerialNumber> serialNumbers)
        {
            ItemCode = itemCode;
            ScanCode = scanCode;
            JAN = jAN;
            Qty = qty;
            Unit = unit;
            SmallestQty = smallestQty;
            SmallestUnit = smallestUnit;
            Cost = cost;
            Price = price;
            Currency = currency;
            Width = width;
            Height = height;
            Length = length;
            Weight = weight;
            Location = location;
            ExpireDate = expireDate;
            ManufacturingDate = manufacturingDate;
            BestBeforeDate = bestBeforeDate;
            ControlLevel1 = controlLevel1;
            ControlLevel2 = controlLevel2;
            ControlLevel3 = controlLevel3;
            Remark = remark;
            _serialNumbers = serialNumbers;
        }

        public IReadOnlyCollection<SerialNumber> SerialNumbers => _serialNumbers;

    }

    public class SerialNumber
    {
        public int Id { get; private set; }
        public string ProjectId { get; private set; }
        public int? ReceiveItemId { get; private set; }
        public int? ItemId { get; private set; }
        public int? Seq { get; private set; }
        public string Number1 { get; private set; }
        public string Number2 { get; private set; }
        public string Number3 { get; private set; }
        public string SerialGroup { get; private set; }

        public SerialNumber(string projectId, int? receiveItemId, int? itemId, int? seq, string number1, string number2, string number3, string serialGroup)
        {
            ProjectId = projectId;
            ReceiveItemId = receiveItemId;
            ItemId = itemId;
            Seq = seq;
            Number1 = number1;
            Number2 = number2;
            Number3 = number3;
            SerialGroup = serialGroup;
        }
    }

    public class SpecialProduct
    {
        public int Id { get; private set; }
        public string ProjectId { get; private set; }
        public string ItemId { get; private set; }
        public int SpecialProductTypeId { get; private set; }
        public string Description { get; private set; }

        public SpecialProductType SpecialProductType { get; private set; }

        public SpecialProduct(int id, string projectId, string itemId, int specialProductTypeId, string description)
        {
            Id = id;
            ProjectId = projectId;
            ItemId = itemId;
            SpecialProductTypeId = specialProductTypeId;
            Description = description;
        }
    }

    public class SpecialProductType
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }

        public SpecialProductType(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }

    public class FileUpload
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
    }

    public class InvoiceItem
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public int Qty { get; set; }
        public bool IsUnknowQty { get; set; }
        public string Remark { get; set; }
    }
}
