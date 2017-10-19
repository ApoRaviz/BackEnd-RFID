using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiPickingFromWinRequest
    {
        public string UserID { get; set; }
        public List<PickingRequest> ListPicking { get; set; }
    }
}
