using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Attributes;

namespace Fuji.Common.ValueObject
{
    public enum FujiStatus
    {
        [ValueEnum("NEW")]
        New,
        [ValueEnum("RECEIVED")]
        Received,
        [ValueEnum("IMP_PICKING")]
        ImpPicking,
        [ValueEnum("EXPORTED")]
        Exported,
        [ValueEnum("SHIPPED")]
        Shipped,
        [ValueEnum("DELETED")]
        Deleted,
        //Detail
        [ValueEnum("SCANNED")]
        Scanned
    }
}
