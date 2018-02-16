using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Master;

namespace Master.Common.ValueObject
{
    public class UnitDto 
    {
        public UnitDto()
        {
            this.Item_Unit = new HashSet<ItemUnitDto>();
        }

        public int UnitIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public byte Active { get; set; }

        public virtual ICollection<ItemUnitDto> Item_Unit { get; set; }
    }
}
