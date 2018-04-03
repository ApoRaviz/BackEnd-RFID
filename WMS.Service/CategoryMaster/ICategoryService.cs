using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Common;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface ICategoryService : IService
    {
        IEnumerable<Category_MT> GetCategories();
        IEnumerable<Category_MT> GetCategoriesByProjectID(int projectID);
        Category_MT GetCategory(int id);
        IEnumerable<AutocompleteCategoryDto> AutocompleteCategory(string term);
        int CreateCategory(Category_MT category);
        bool UpdateCategory(Category_MT category);
        bool DeleteCategory(int id);        
    }
}
