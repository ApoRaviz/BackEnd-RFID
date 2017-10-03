using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WMS.Master.Unit;
using WMS.Repository;

namespace WMS.Master
{
    public class ItemUnitDto : IEntityDto
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
        public byte MainUnit { get; set; }
        public byte Sequence { get; set; }
        public short QtyInParent { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<decimal> Price { get; set; }

    }
}
