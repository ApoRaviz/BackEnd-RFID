using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Master;
using WMS.Master.Unit;
using WMS.Repository;

namespace WMS.Master
{
    public class ItemSetDetailDto : BaseEntityDto
    {
        public int IDSys { get; set; }
        public int ItemIDSys { get; set; }
        public int ItemSetIDSys { get; set; }
        public string ItemCode { get; set; }
        public int Qty { get; set; }
        public string ItemName { get; set; }
    }
}
