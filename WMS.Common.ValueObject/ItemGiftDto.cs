using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class ItemGiftDto
    {
        public int ItemIDSys { get; set; }
        public int UnitIDSys { get; set; }
        public int SupIDSys { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }

    }
}
