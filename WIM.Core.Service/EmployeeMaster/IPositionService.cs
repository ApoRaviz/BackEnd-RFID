using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Employee;

namespace WIM.Core.Service.EmployeeMaster
{
    public interface IPositionService : IService
    {
        IEnumerable<Positions> GetPositions();
        Positions GetPositionByPositionIDSys(int id);
        Positions SetPositionConfig(int id , List<PositionConfig<List<PositionConfig<string>>>> positionConfig);
        int CreatePosition(Positions position);
        bool UpdatePosition(Positions position);
        bool DeletePosition(int id);
    }
}
