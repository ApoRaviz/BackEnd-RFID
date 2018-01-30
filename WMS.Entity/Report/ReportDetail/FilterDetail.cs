using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Report
{
    public class FilterDetail
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string DataType { get; set; }
        public string Operator { get; set; }
        public string Condition1 { get; set; }
        public string Condition2 { get; set; }
    }
}
