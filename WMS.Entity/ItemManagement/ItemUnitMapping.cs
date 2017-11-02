using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.ItemManagement
{
    [Table("ItemUnitMapping")]
    public class ItemUnitMapping : BaseEntity
    {
        [Key]
        public int ItemIDSys { get; set; }
        public int UnitIDSys { get; set; }
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public byte MainUnit { get; set; }
        public byte Sequence { get; set; }
        public short QtyInParent { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<decimal> Price { get; set; }

        public virtual Item_MT Item_MT { get; set; }
        public virtual Unit_MT Unit_MT { get; set; }
    }
}
