using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Common;

namespace WMS.Common.ValueObject
{
    public class LogMasterPaging
    {
        public LogMasterPaging(int totalrow, IEnumerable<UserLogDto> logData)
        {
            this.Totalrow = totalrow;
            this.logData = logData;
        }
        public int Totalrow { get; set; }
        public IEnumerable<UserLogDto> logData { get; set; }
    }
}
