using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;


namespace WIM.Core.Service
{
    public interface IMenuProjectMappingService : IService
    {
        IEnumerable<MenuDto> GetMenuDtoByProjectID(int id);
        IEnumerable<MenuProjectMapping> GetMenuProjectMapping();
        IEnumerable<MenuProjectMappingDto> GetMenuProjectMappingByID(int id);
        IEnumerable<MenuProjectMappingDto> GetAllMenu(int projectid , IEnumerable<MenuProjectMappingDto> menu);
        IEnumerable<MenuProjectMappingDto> GetAllMenu(int projectid, IEnumerable<MenuProjectMappingDto> menu,CoreDbContext x);
        IEnumerable<MenuProjectMappingDto> GetMenuPermission(string userid,int projectid);
        IEnumerable<MenuDto> GetMenuDtoDefault(int i);
        /// <summary>
        /// use in genmenu at controller
        /// comment by oil
        /// </summary>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        IEnumerable<MenuProjectMappingDto> GetMenuProjectByID(int id ,CoreDbContext x);
        /// <summary>
        /// use in genmenu at controller comment by oil
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        IEnumerable<MenuProjectMappingDto> GetMenuProjectPermission(string userid, int projectid, CoreDbContext x);
        int CreateMenuProjectMapping(MenuDto MenuProjectMapping, int projectID,byte sort);
        int CreateMenuProjectMapping(MenuProjectMapping MenuProjectMapping);
        bool UpdateMenuProjectMapping(MenuProjectMapping MenuProjectMapping);
        bool UpdateMenuProjectMapping(List<MenuProjectMappingDto> MenuProjectMapping);
        bool DeleteMenuProjectMapping(List<MenuProjectMappingDto> id);        
    }
}
