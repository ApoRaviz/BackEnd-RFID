using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.ValueObject
{
    public class TableColumnsDescription
    {
        public string Column_Name { get; set; }
        public string Data_type { get; set; }
        public short Max_Length { get; set; }
        public byte precision { get; set; }
        public byte scale { get; set; }
        public Nullable<bool> is_nullable { get; set; }
        public bool Primary_Key { get; set; }
    }
}
