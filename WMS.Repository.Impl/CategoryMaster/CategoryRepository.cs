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
using WMS.Context;
using WMS.Entity.ItemManagement;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WMS.Common.ValueObject;

namespace WMS.Repository.Impl
{
    public class CategoryRepository : Repository<Category_MT> , ICategoryRepository
    {
        private WMSDbContext Db { get; set; }

        public CategoryRepository(Context.WMSDbContext context):base(context)
        {
            Db = context;
        }

        public IEnumerable<AutocompleteCategoryDto> AutocompleteCategory(string term)
        {
            var qr = (from cm in Db.Category_MT
                      where cm.CateID.Contains(term)
                      || cm.CateName.Contains(term)
                      select new AutocompleteCategoryDto
                      {
                          CateIDSys = cm.CateIDSys,
                          CateName = cm.CateName,
                          CateID = cm.CateID
                      }
                     ).ToList();
            return qr;
        }

    }

}
