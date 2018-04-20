using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class GroupLocationDto
    {
        public int GroupLocIDSys { get; set; }
        public int LocTypeIDSys { get; set; }
        public int WHIDSys { get; set; }
        public string WHName { get; set; }
        public string LocTypeName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Left { get; set; }
        public int? Top { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public int? Height { get; set; }
        public int? ZoneIDSys { get; set; }
        public int? Floor { get; set; }
        public int? ZoneID { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string StyleObject { get; set; }
        //public virtual IEnumerable<Location> Location { get; set; }
    }
}
