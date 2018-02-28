using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WIM.Core.Service.EmployeeMaster
{
    public interface IProbationService : IService
    {
        IEnumerable<Probation_MT> GetProbation();
        Probation_MT GetProbationByProbationIDSys(int id);
        int CreateProbation(Probation_MT resign);
        bool UpdateProbation(Probation_MT resign);
        bool DeleteProbation(int id);
    }
}
