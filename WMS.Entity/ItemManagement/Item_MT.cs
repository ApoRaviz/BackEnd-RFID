using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.SupplierManagement;

namespace WMS.Entity.ItemManagement
{
    [Table("Item_MT")]
    public class Item_MT : BaseEntity
    {
        public Item_MT()
        {
            this.ItemInspectMapping = new HashSet<ItemInspectMapping>();
            this.ItemUnitMapping = new HashSet<ItemUnitMapping>();
            this.ItemSetDetails = new HashSet<ItemSetDetail>();
        }

        [Key]
        public int ItemIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public int? CateIDSys { get; set; }
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
        public Nullable<bool> SerialControl { get; set; }
        public Nullable<bool> InspectControl { get; set; }
        public Nullable<bool> ExpireControl { get; set; }
        public Nullable<bool> DimensionControl { get; set; }
        public Nullable<bool> BoxControl { get; set; }
        public Nullable<bool> LotControl { get; set; }
        public Nullable<bool> PalletControl { get; set; }
        public Nullable<bool> ItemSetControl { get; set; }
        public Nullable<int> MiniAlert { get; set; }
        public Nullable<short> AlertExp { get; set; }
        public string TaxCond { get; set; }
        public byte TaxPerc { get; set; }
        public Nullable<int> ItemSetIDSys { get; set; }
        public string Remark { get; set; }
        public Nullable<int> UnitMinStock { get; set; }
        public Nullable<System.DateTime> ActiveDateFrom { get; set; }
        public Nullable<System.DateTime> ActiveDateTo { get; set; }
        public Nullable<byte> InspectTypeIDSys { get; set; }

        public virtual ICollection<ItemInspectMapping> ItemInspectMapping { get; set; }
        [ForeignKey("ProjectIDSys")]
        public virtual Project_MT Project_MT { get; set; }
        [ForeignKey("SupIDSys")]
        public virtual Supplier_MT Supplier_MT { get; set; }
        [ForeignKey("ItemSetIDSys")]
        public virtual ItemSet_MT ItemSet_MT { get; set; }
        [ForeignKey("CateIDSys")]
        public virtual Category_MT Category_MT { get; set; }
        public virtual ICollection<ItemUnitMapping> ItemUnitMapping { get; set; }
        public virtual ICollection<ItemSetDetail> ItemSetDetails { get; set; }
    }
}
