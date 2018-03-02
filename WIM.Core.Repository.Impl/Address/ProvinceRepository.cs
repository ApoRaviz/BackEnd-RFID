using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.Address;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class ProvinceRepository : Repository<Province_MT>, IProvinceRepository
    {
        private CoreDbContext Db;
        public ProvinceRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }
    }
}
