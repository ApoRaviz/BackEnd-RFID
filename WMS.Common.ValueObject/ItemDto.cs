using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.SupplierManagement;
using WMS.Entity.InspectionManagement;
using WMS.Entity.ItemManagement;
using WMS.Master;

namespace WMS.Common.ValueObject
{
    public class ItemDto
    {
        public ItemDto()
        {
            this.ItemUnitMapping = new HashSet<ItemUnitDto>();
        }

        public int ItemIDSys { get; set; }        
        public string ItemCode { get; set; }
        public string JAN { get; set; }
        public string ScanCode { get; set; }
        public string ItemName { get; set; }
        public string ItemColor { get; set; }
        public string Description { get; set; }
        public string ExpControl { get; set; }
        public bool SerialControl { get; set; }
        public string SerialDigit { get; set; }
        public string SerialFormat { get; set; }
        public string SupIDSys { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }
        public string InspectControl { get; set; }
        public string ExpireControl { get; set; }
        public string DimensionControl { get; set; }
        public string BoxControl { get; set; }
        public string LotControl { get; set; }
        public string PalletControl { get; set; }
        public string ItemSetControl { get; set; }
        public string MiniAlert { get; set; }
        public string AlertExp { get; set; }
        public string TaxCond { get; set; }
        public byte TaxPerc { get; set; }
        public byte Active { get; set; }
        public string Remark { get; set; }
        public int InspectTypeIDSys { get; set; }

        public int ProjectIDSys { get; set; }
        public ProjectDto Project_MT { get; set; }
        public Supplier_MT Supplier_MT { get; set; }
        public ICollection<ItemUnitDto> ItemUnitMapping { get; set; }
        public ICollection<object> ItemInspectMapping { get; set; }
    }
}
