using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using WIM.Core.Context;
using WIM.Core.Entity;
using WIM.Core.Repository;


namespace WIM.Core.Repository.Impl
{
    public class EmployeeRepository : Repository<Employee_MT> , IEmployeeRepository
    {
        private CoreDbContext Db { get; set; }

        public EmployeeRepository(CoreDbContext context): base(context)
        {
            Db = context;
        }

    }
}
