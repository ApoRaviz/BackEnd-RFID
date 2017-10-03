using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Master;
using WMS.Master.Menu;

namespace WMS.Master
{
    public interface IMenuProjectMappingService
    {
        IEnumerable<MenuDto> GetMenuDtoByProjectID(int id);
        IEnumerable<MenuProjectMapping> GetMenuProjectMapping();
        IEnumerable<MenuProjectMappingDto> GetMenuProjectMappingDto(int id);
        IEnumerable<MenuProjectMappingDto> GetMenuProjectMappingByID(int id);
        IEnumerable<MenuProjectMappingDto> GetAllMenu(int projectid , IEnumerable<MenuProjectMappingDto> menu);
        IEnumerable<MenuProjectMappingDto> GetMenuPermission(string userid,int projectid);
        IEnumerable<MenuDto> GetMenuDtoDefault(int i);
        int CreateMenuProjectMapping(MenuDto MenuProjectMapping, int projectID,byte sort);
        int CreateMenuProjectMapping(MenuProjectMapping MenuProjectMapping);
        bool UpdateMenuProjectMapping(int id, MenuProjectMapping MenuProjectMapping);
        bool UpdateMenuProjectMapping(List<MenuProjectMappingDto> MenuProjectMapping);
        bool DeleteMenuProjectMapping(List<MenuProjectMappingDto> id);        
    }
}
