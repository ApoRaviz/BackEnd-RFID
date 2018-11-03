using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.ValueObject
{
    public class FujiBoxNumberAndAmountModel
    {
        public int ItemIndex { get; set; }
        public string BoxNumber { get; set; }
        public int Amount { get; set; }
        public int Type { get; set; }
    }
}
