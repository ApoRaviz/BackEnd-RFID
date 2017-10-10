using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.ItemManagement
{
    public class ItemSetDetail
    {
        public int IDSys { get; set; }
        public int ItemSetIDSys { get; set; }
        public int ItemIDSys { get; set; }
        public int Qty { get; set; }

        public virtual Item_MT Item_MT { get; set; }
        public virtual ItemSet_MT ItemSet_MT { get; set; }
    }
}
