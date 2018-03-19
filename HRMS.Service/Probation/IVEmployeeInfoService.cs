using HRMS.Entity.Probation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;

namespace HRMS.Service.Probation
{
    public interface IVEmployeeInfoService : IService
    {
        IEnumerable<VEmployeeInfo> GetProbation();
        //Person_MT GetPersonByPersonIDSys(string id);
        //PersonDto GetPersonByPersonID(int id);
        //int CreatePerson(Person_MT Person);
        //bool UpdatePerson(Person_MT Person);
        //bool UpdatePersonByID(Person_MT Person);
        //bool DeletePerson(int id);
    }
}
