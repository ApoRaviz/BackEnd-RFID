﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Person;
using WMS.Common;

namespace WMS.Service
{
    public interface IPersonService
    {
        IEnumerable<Person_MT> GetPersons();
        Person_MT GetPersonByPersonIDSys(string id);
        PersonDto GetPersonByPersonID(int id);
        int CreatePerson(Person_MT Person);
        bool UpdatePerson(string id, Person_MT Person);
        bool UpdatePersonByID(Person_MT Person);
        bool DeletePerson(int id);        
    }
}
