using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class IsuzuTagReport
    {
        public string MonthName { get; set; }

        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }
        public int ReceivedNumber { get; set; }
        public int ShippedNumber { get; set; }
        public int TotalNumber { get; set; }
    }
}
