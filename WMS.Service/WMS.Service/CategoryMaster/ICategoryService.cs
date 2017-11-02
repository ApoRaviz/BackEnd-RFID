using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;

namespace WMS.Service
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetCategories();
        IEnumerable<CategoryDto> GetCategoriesByProjectID(int projectID);
        CategoryDto GetCategory(int id);
        int CreateCategory(Category_MT category);
        bool UpdateCategory(Category_MT category);
        bool DeleteCategory(int id);        
    }
}
