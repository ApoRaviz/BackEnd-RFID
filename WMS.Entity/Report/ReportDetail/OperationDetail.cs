using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Report
{
    public class OperationDetail
    {
        public ColumnDetail ColumnFirst { get; set; }
        public ColumnDetail ColumnSecond { get; set; }
        public string Operator { get; set; }
        public string AliasName { get; set; }
        
    }
}
