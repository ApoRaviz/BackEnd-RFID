using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WIM.Core.Service
{
    public interface IEmployeeService
    {
        IEnumerable<Employee_MT> GetEmployees();
        Employee_MT GetEmployeeByEmployeeIDSys(string id);
        Employee_MT GetEmployeeByPerson(int id);
        string CreateEmployee(Employee_MT Employee);
        bool UpdateEmployee(string id, Employee_MT Employee);
        bool UpdateEmployeeByID(Employee_MT Employee);
        bool DeleteEmployee(string id);        
    }
}
