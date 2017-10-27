using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;


namespace WIM.Core.Service
{
    public interface IMenuProjectMappingService
    {
        IEnumerable<MenuDto> GetMenuDtoByProjectID(int id);
        IEnumerable<MenuProjectMapping> GetMenuProjectMapping();
        IEnumerable<MenuProjectMappingDto> GetMenuProjectMappingByID(int id);
        IEnumerable<MenuProjectMappingDto> GetAllMenu(int projectid , IEnumerable<MenuProjectMappingDto> menu);
        IEnumerable<MenuProjectMappingDto> GetMenuPermission(string userid,int projectid);
        IEnumerable<MenuDto> GetMenuDtoDefault(int i);
        int CreateMenuProjectMapping(MenuDto MenuProjectMapping, int projectID,byte sort,string username);
        int CreateMenuProjectMapping(MenuProjectMapping MenuProjectMapping , string username);
        bool UpdateMenuProjectMapping(MenuProjectMapping MenuProjectMapping,string username);
        bool UpdateMenuProjectMapping(List<MenuProjectMappingDto> MenuProjectMapping,string username);
        bool DeleteMenuProjectMapping(List<MenuProjectMappingDto> id);        
    }
}
