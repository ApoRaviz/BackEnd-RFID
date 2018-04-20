using HRMS.Entity.Probation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WIM.Core.Entity.PositionConfigManagement;
using WIM.Core.Service;

namespace HRMS.Service.Probation
{
    public interface IVEmployeeInfoService : IService
    {
        IEnumerable<VEmployeeInfo> GetProbation();
        IEnumerable<VEmployeeInfo> GetEmployeetoEvaluate();
        VEmployeeInfo GetEmployeeByEmployeeIDSys(string id);
        //Employee_MT SetPositionConfig2(int id, PositionConfig positionConfig);
        bool UpdateEmployeeByID(VEmployeeInfo Employee);
        //PersonDto GetPersonByPersonID(int id);
        //int CreatePerson(Person_MT Person);
        //bool UpdatePerson(Person_MT Person);
        //bool UpdatePersonByID(Person_MT Person);
        //bool DeletePerson(int id);
    }
}
