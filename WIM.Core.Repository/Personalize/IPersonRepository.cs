using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.View;

namespace WIM.Core.Repository
{
    public interface IPersonRepository : IRepository<Person_MT>
    {
        IEnumerable<VPersons> GetList();
    }
}
