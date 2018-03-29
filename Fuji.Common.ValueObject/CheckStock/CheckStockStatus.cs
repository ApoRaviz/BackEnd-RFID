using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Attributes;

namespace Fuji.Common.ValueObject.CheckStock
{
    public enum CheckStockStatus
    {
        [ValueEnum("New")]
        New,
        [ValueEnum("InProgress")]
        InProgress,
        [ValueEnum("Completed")]
        Completed
    }
}
