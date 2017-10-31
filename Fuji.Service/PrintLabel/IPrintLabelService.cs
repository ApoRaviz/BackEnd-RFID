using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fuji.Service.PrintLabel
{
    public interface IPrintLabelService
    {        
        int GetRunningByType(string type, int running,string userUpdate);
        StreamContent GetReportStream(int running, int baseRunning, string type = "BXFJ");
    }
}
