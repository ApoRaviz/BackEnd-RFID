using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using WIM.Core.Common.Utility.Attributes;

namespace WIM.Core.Common.Utility.Validation
{
    public enum ValidationEnum
    {
        [ValueEnum("A01")]
        Required,
        [ValueEnum("B01")]
        Email,
        [ValueEnum("C01")]
        MaxLength,
        [ValueEnum("C02")]
        MinLength,
    }
}
