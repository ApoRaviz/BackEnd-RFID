using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WIM.Core.Context
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(string dbName) : base(dbName)
        {

        }

        public BaseDbContext(string dbName, string methodName) : base(dbName)
        {
            DebugWriteLog(methodName);
        }       

        public void DebugWriteLog(string methodName)
        {
            Debug.WriteLine(string.Format("/**********************#DEBUGSQL {0} ********************************", methodName));
            Database.Log = (s) => Debug.WriteLine(s);
        }
    }
}
