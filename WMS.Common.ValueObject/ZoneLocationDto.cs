using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class ZoneLocationDto
    {
        public string ZoneName { get; set; }
        public string WHName { get; set; }
        public string Type { get; set; }
        public double? AvailableArea { get; set; }
        public int LocIDSys { get; set; }
        public string LocNo { get; set; }

    }
}
