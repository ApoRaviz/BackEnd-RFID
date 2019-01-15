using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Entity.Shipping
{
    public class AllocateView
    {
        public int AllocationId { get; set; }
        public string ProjectId { get; set; }
        public string AllocationNumber { get; set; }

        public int? AllocationStatusId { get; set; }
        public string AllocationStatusDisplay { get; set; }
        public DateTime? AllocationDate { get; set; }
        public decimal? AllocationQty { get; set; }
        public decimal? InspectQty { get; set; }
        public string Remark { get; set; }

        public List<AllocateDetailView> AllocateDetails { get; set; }

    }

    public class AllocateDetailView
    {
        public int AllocationDetailId { get; set; }
        public int? AllocationId { get; set; }
        public int? InventoryId { get; set; }
        public int? OrderDetailId { get; set; }

        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }

        public int? AllocationDetailStatusId { get; set; }
        public string OrderStatusDisplay { get; set; }
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ScanCode { get; set; }
        public string JAN { get; set; }
        public string ItemName { get; set; }
        public decimal? Qty { get; set; }
        public decimal? InspectQty { get; set; }
        public int? UnitId { get; set; }
        public int? SmallestUnitId { get; set; }
        public decimal? SmallestQty { get; set; }
        public string WarehouseCode { get; set; }
        public string QualityTypeCode { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public DateTime? OrderDetailCancelledDate { get; set; }
        public int? LocationId { get; set; }
        public string LocationCode { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? BestBeforeDate { get; set; }
        public string ControlLevel1 { get; set; }
        public string ControlLevel2 { get; set; }
        public string ControlLevel3 { get; set; }
        public string SerialNumber1 { get; set; }
        public string SerialNumber2 { get; set; }
        public string SerialNumber3 { get; set; }
        public string Remark { get; set; }
        public List<ItemInspectViewModel> ItemInspects { get; set; }
    }

    public class ItemInspectViewModel
    {
        public int InspectId { get; set; }
        public int ItemId { get; set; }
        public string InspectColumnName { get; set; }
        public string InspectDisplay { get; set; }
    }
}
