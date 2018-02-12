using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Employee;

namespace WIM.Core.Service.EmployeeMaster
{
    public interface IProbationService : IService
    {
        IEnumerable<Probation_MT> GetProbation();
        Probation_MT GetProbationByEmID(int id);
        int CreateProbation(Probation_MT resign);
        bool UpdateProbation(Probation_MT resign);
        bool Delete(Probation_MT id);
    }
}
