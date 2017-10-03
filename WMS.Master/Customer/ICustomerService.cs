using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Master.Customer
{
    public interface ICustomerService
    {
        object GetCustomers(string userid);
        object GetProjectByCustomer(string userid,int cusIDSys);
        CustomerDto GetCustomersInclude(int id, string[] tableNames);
        ProcGetCustomerByCusIDSys_Result GetCustomerByCusIDSys(int id);
        CustomerDto GetCustomerByCusIDSysIncludeProjects(int id);
        int CreateCustomer(Customer_MT customer);
        bool UpdateCustomer(int id, Customer_MT customer);
        bool DeleteCustomer(int id);
    }
}
