using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;


namespace WIM.Core.Service
{
    public interface IMenuService
    {
        IEnumerable<Menu_MT> GetMenu();
        IEnumerable<MenuDto> GetMenuDto();
        IEnumerable<MenuDto> GetMenuDto(int projectIDSys);
        IEnumerable<MenuDto> GetMenuDtoNotHave(int projectIDSys);
        Menu_MT GetMenuByMenuIDSys(int id);
        IEnumerable<MenuDto> GetMenuByMenuParentID(int id);
        int CreateMenu(MenuDto Menu, int projectID,byte sort);
        int CreateMenu(Menu_MT Menu);
        bool UpdateMenu(Menu_MT Menu );
        bool UpdateMenu(List<MenuDto> menu);
        bool DeleteMenu(int id);        
    }
}
