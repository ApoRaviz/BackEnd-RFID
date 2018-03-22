using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Report
{
    public class ColumnDetail
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string AliasName { get; set; }
        public int ColumnOrder { get; set; }
    }
}
