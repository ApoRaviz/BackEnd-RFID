using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Report
{
    public class TableDetail
    {
        public string TableName { get; set; }
        public List<ColumnDetail> Column { get; set; }
        
    }
}
