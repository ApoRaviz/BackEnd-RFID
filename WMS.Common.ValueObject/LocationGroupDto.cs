using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master.Common.ValueObject
{
    public class LocationGroupDto
    {
        public int GroupLocIDSys { get; set; }
        public int LocTypeIDSys { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Left { get; set; }
        public double? Top { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public int? ZoneIDSys { get; set; }
        public int? Floor { get; set; }
        public int? ZoneID { get; set; }
        public byte Rows { get; set; }
        public byte Columns { get; set; }
        
        public string DimUnit { get; set; }
        public string DimWidth { get; set; }
        public string DimLength { get; set; }
        public string DimHeight { get; set; }
        public string DimColor { get; set; }
    }
}
