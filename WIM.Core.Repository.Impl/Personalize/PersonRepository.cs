﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WIM.Core.Context;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.View;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class PersonRepository : Repository<Person_MT> , IPersonRepository
    {
        private CoreDbContext Db { get; set; }
        public PersonRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }

        public Person_MT GetByUserID(string id)
        {
            var person = (from i in Db.Person_MT
                          where (from o in Db.User
                                 where o.UserID == id
                                 select o.PersonIDSys)
                               .Contains(i.PersonIDSys)
                          select i).SingleOrDefault();
            return person;
        }

        public IEnumerable<VPersons> GetList()
        {
            var persons = from i in Db.VPersons
                          select i;
            return persons.ToList();
        }

    }
}
