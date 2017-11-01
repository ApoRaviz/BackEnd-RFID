using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using WIM.Core.Context;
using WIM.Core.Entity.Person;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class PersonRepository : Repository<Person_MT> , IPersonRepository
    {
        private CoreDbContext Db { get; set; }
        private IIdentity User { get; set; }
        public PersonRepository(CoreDbContext context,IIdentity identity): base(context,identity)
        {
            Db = context;
            User = identity;
        }

        public IEnumerable<Person_MT> Get()
        {
            var person = from c in Db.Person_MT
                         select c;
            return person.ToList();
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

    }
}
