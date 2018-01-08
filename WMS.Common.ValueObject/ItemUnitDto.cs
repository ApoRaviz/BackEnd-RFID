using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WMS.Common.ValueObject
{
    public class ItemUnitDto
    {

        public int ItemIDSys { get; set; }
        public int UnitIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public bool MainUnit { get; set; }
        public byte Sequence { get; set; }
        public short QtyInParent { get; set; }

        [NotMapped]
        public short UID { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> CurrencyIDSys { get; set; }

    }
}
