using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WIM.Core.Context;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.View;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class PersonEmailRepository : Repository<Person_Email> , IPersonEmailRepository
    {
        private CoreDbContext Db { get; set; }
        public PersonEmailRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }


    }
}
