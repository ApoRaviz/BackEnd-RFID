using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.ExternallInterface
{
    public interface IHelperService
    {
        void InsertErrorLog(ErrorLogs errorLog);
    }
}
