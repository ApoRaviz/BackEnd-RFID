using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.ItemManagement
{
    public class ItemUnitMapping
    {
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
