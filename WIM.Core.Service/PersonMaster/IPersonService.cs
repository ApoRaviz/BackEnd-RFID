using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.View;

namespace WIM.Core.Service
{
    public interface IPersonService : IService
    {
        IEnumerable<VPersons> GetPersons();
        Person_MT GetPersonByPersonIDSys(string id);
        PersonDto GetPersonByPersonID(int id);
        int CreatePerson(Person_MT Person);
        bool UpdatePerson(Person_MT Person);
        bool UpdatePersonByID(Person_MT Person);
        bool DeletePerson(int id);        
    }
}
