using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class TableColumnDetail
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int ColumnOrder { get; set; }
        public string AliasName { get; set; }
    }
}
