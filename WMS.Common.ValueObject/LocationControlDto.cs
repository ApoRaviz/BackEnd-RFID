using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class LocationControlDto
    {
        public double Dimension { get; set; }
        public List<ControlValueDto> Control { get; set; }
    }
}
