using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Common.SqlLog
{
    public class LogDBConfiguration: DbConfiguration
    {
        public LogDBConfiguration()
        {
            SetDatabaseLogFormatter(
                (context, writeAction) => new MyFormatter(context, writeAction));
        }
    }
}
