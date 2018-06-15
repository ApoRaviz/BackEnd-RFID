using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Attributes;

namespace Isuzu.Common.ValueObject
{
    public enum IsuzuStatus
    {
        [ValueEnum("IMPORTED")]
        Imported,
        [ValueEnum("NEW")]
        New,
        [ValueEnum("REGISTERED_YUT")]
        RegisteredAtYUT,
        [ValueEnum("REGISTERED_ITA")]
        RegisteredAtITA,
        [ValueEnum("RECEIVED")]
        Received,        
        [ValueEnum("HOLD")]
        Hold,
        [ValueEnum("SHIPPED")]
        Shipped,
        [ValueEnum("DELETED")]
        Deleted,
        [ValueEnum("RECEIVED_YUT")]
        ReceivedAtYUT,
        [ValueEnum("RECEIVED_ITA")]
        ReceivedAtITA,

        //Addition
        [ValueEnum("REGISTERED")]
        Registered,
        [ValueEnum("CARTON_PACKED")]
        CartonPacked,
        [ValueEnum("CASE_PACKED")]
        CasePacked,
        [ValueEnum("REGISTERED_PARTIAL")]
        RegisteredPartial,
        [ValueEnum("CARTON_PACKED_PARTIAL")]
        CartonPackedPartial,
        [ValueEnum("CASE_PACKED_PARTIAL")]
        CasePackedPartial,
    }
}
