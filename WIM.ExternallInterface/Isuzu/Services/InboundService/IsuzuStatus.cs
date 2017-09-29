using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.ExternallInterface.Isuzu.Services.InboundService
{
    public enum IsuzuStatus
    {
      NEW = 1,
      RECEIVE = 2,
      IMPORTED = 3,
      HOLD = 4,
      SHIPPED = 5,
      DELETED =6,
    }
}
