using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Master;
using WMS.Repository;

namespace WMS.Common
{
    public class ApiMTDto : BaseEntityDto
    {
        public string ApiIDSys { get; set; }
        public string Api { get; set; }
        public string Method { get; set; }
        public string Controller { get; set; }
    }
}
