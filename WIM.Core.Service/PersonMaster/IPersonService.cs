using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Person;

namespace WIM.Core.Service
{
    public interface IPersonService
    {
        IEnumerable<Person_MT> GetPersons();
        Person_MT GetPersonByPersonIDSys(string id);
        PersonDto GetPersonByPersonID(int id);
        int CreatePerson(Person_MT Person,string username);
        bool UpdatePerson(Person_MT Person,string username);
        bool UpdatePersonByID(Person_MT Person,string username);
        bool DeletePerson(int id , string username);        
    }
}
