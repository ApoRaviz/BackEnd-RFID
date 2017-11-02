using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;

namespace WIM.Core.Repository
{
    public interface IMenuProjectMappingRepository : IRepository<MenuProjectMapping>
    {
        IEnumerable<MenuProjectMappingDto> GetAllMenu(int id, IEnumerable<MenuProjectMappingDto> menu);
        IQueryable<MenuProjectMapping> GetMenuPermission(string userid, int projectid);
    }
}
