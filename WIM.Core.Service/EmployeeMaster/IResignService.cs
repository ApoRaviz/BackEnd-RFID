using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Employee;

namespace WIM.Core.Service.EmployeeMaster
{
    public interface IResignService : IService
    {
        IEnumerable<Resign> GetResign();
        Resign GetResignByEmID(string id);
        string CreateResign(Resign resign);
        bool UpdateResign(Resign resign);
        bool DeleteResign(int id);
    }
}
