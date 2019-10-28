using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isuzu.Common.ValueObject
{
    public class GeneralLogModel
    {
        public int LogIDSys { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string RefID { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }


        public string StatusDetail { get; set; }
    }
}
