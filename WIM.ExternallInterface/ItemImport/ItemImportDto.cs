using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WIM.Repositories;

namespace WIM.ExternallInterface
{
    public class ItemImportDto : BaseEntityDto
    {
        public string HeadID { get; set; }
        public string ItemCode { get; set; }
        public int Qty { get; set; }

    }
}
