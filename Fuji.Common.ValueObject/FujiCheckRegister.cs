using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiCheckRegister
    {
        public DateTime? LastestDate { get; set; }
        public int TotalRecord { get; set; }
        public IEnumerable<FujiBoxNumberAndAmountModel> BoxAndAmount { get; set; }
    }
}
