using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;
using WMS.Repository;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace WMS.Repository.Impl
{
    public class CategoryRepository : Repository<Category_MT> , ICategoryRepository
    {
        private WMSDbContext Db { get; set; }
        private IIdentity user { get; set; }

        public CategoryRepository(WMSDbContext context,IIdentity identity):base(context,identity)
        {
            Db = context;
            user = identity;
        }

       

    }

}
