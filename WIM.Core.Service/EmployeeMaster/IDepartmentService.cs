using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Employee;

namespace WIM.Core.Service.EmployeeMaster
{
    public interface IDepartmentService : IService
    {
        IEnumerable<Departments> GetDepartments();
        Departments GetDepartmentByDepIDSys(int id);
        int CreateDepartment(Departments department);
        bool UpdateDepartment(Departments department);
        bool DeleteDepartment(int id);
    }
}
