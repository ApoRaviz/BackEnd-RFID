using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.CustomerManagement;
using WMS.Common;

namespace WMS.Service
{
    public interface ICustomerService
    {
        object GetCustomers(string userid);
        object GetProjectByCustomer(string userid,int cusIDSys);
        object GetCustomerAll();
        CustomerDto GetCustomersInclude(int id, string[] tableNames);
        Customer_MT GetCustomerByCusIDSys(int id);
        CustomerDto GetCustomerByCusIDSysIncludeProjects(int id);
        int CreateCustomer(Customer_MT customer);
        bool UpdateCustomer(int id, Customer_MT customer);
        bool DeleteCustomer(int id);
    }
}
