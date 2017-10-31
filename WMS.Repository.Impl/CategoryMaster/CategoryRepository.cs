using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Repository;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;

namespace WMS.Repository.Impl
{
    public class CategoryRepository : Repository<Category_MT> , ICategoryRepository
    {
        private WMSDbContext Db { get; set; }

        public CategoryRepository(WMSDbContext context):base(context)
        {
            Db = context;
        }

       

    }

}
