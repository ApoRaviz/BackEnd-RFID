using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public enum FujiStatus
    {
        NEW = 1,
        RECEIVED = 2,
        IMP_PICKING = 3,
        EXPORTED = 4,
        SHIPPED = 5,
        DELETED = 6,
        //Detail
        SCANNED = 7
    }
}
