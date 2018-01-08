using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.CustomerManagement;

namespace WIM.Core.Repository
{
    public interface ICustomerRepository : IRepository<Customer_MT>
    {
        object GetByUserID(string userid);
        object GetProjectByUserIDCusID(string userid, int cusIDSys);
        IEnumerable<AutocompleteCustomerDto> AutocompleteItem(string term);
    }
}
