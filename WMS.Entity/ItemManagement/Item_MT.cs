using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.SupplierManagement;

namespace WMS.Entity.ItemManagement
{
    public class Item_MT
    {
        public Item_MT()
        {
            this.ItemInspectMapping = new HashSet<ItemInspectMapping>();
            this.ItemUnitMapping = new HashSet<ItemUnitMapping>();
            this.ItemSetDetails = new HashSet<ItemSetDetail>();
        }

        public int ItemIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string ItemCode { get; set; }
        public string JAN { get; set; }
        public string ScanCode { get; set; }
        public string ItemName { get; set; }
        public string ItemColor { get; set; }
        public string Description { get; set; }
        public string SerialFormat { get; set; }
        public int SupIDSys { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }
        public Nullable<byte> SerialControl { get; set; }
        public Nullable<byte> InspectControl { get; set; }
        public Nullable<byte> ExpireControl { get; set; }
        public Nullable<byte> DimensionControl { get; set; }
        public Nullable<byte> BoxControl { get; set; }
        public Nullable<byte> LotControl { get; set; }
        public Nullable<byte> PalletControl { get; set; }
        public Nullable<byte> ItemSetControl { get; set; }
        public Nullable<int> MiniAlert { get; set; }
        public Nullable<short> AlertExp { get; set; }
        public string TaxCond { get; set; }
        public byte TaxPerc { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }
        public Nullable<int> ItemSetIDSys { get; set; }
        public string Remark { get; set; }
        public Nullable<int> UnitMinStock { get; set; }
        public Nullable<System.DateTime> ActiveDateFrom { get; set; }
        public Nullable<System.DateTime> ActiveDateTo { get; set; }
        public Nullable<byte> InspectTypeIDSys { get; set; }

        public virtual ICollection<ItemInspectMapping> ItemInspectMapping { get; set; }
        public virtual Project_MT Project_MT { get; set; }
        public virtual Supplier_MT Supplier_MT { get; set; }
        public virtual ICollection<ItemUnitMapping> ItemUnitMapping { get; set; }
        public virtual ICollection<ItemSetDetail> ItemSetDetails { get; set; }
    }
}
