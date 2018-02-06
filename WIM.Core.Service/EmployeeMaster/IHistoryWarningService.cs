using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Employee;

namespace WIM.Core.Service.EmployeeMaster
{
    public interface IHistoryWarningService : IService
    {
        IEnumerable<HistoryWarning> GetHistories();
        IEnumerable<HistoryWarning> GetHistoryByEmID(string id);
        int CreateHistory(HistoryWarning department);
        bool UpdateHistory(HistoryWarning department);
        bool DeleteHistory(int id);
    }
}
