using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WIM.Core.Entity.PositionConfigManagement;
namespace WIM.Core.Service
{
    public interface IEmployeeService : IService
    {
        IEnumerable<Employee_MT> GetEmployees();
        Employee_MT GetEmployeeByEmployeeIDSys(string id);
        Employee_MT GetEmployeeByPerson(int id);
        string CreateEmployee(Employee_MT Employee);
        bool UpdateEmployee(Employee_MT Employee);
        bool UpdateEmployeeByID(Employee_MT Employee);
        bool DeleteEmployee(string id);
        Employee_MT SetPositionConfig2(string id, WelfareConfig positionConfig);

    }
}
