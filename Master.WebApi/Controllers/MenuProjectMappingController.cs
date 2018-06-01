using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Service;


namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/MenuProjectMappings")]
    public class MenuProjectMappingController : ApiController
    {
        private IMenuProjectMappingService MenuProjectMappingService;
        private IMenuService MenuService;
        private IPermissionService PermissionService;
        private List<MenuProjectMappingDto> Menu;
        private MenuProjectMappingDto MenuParent;
        private IEnumerable<MenuProjectMappingDto> MenuPermis;

        public MenuProjectMappingController(IMenuProjectMappingService MenuProjectMappingService, IMenuService MenuService, IPermissionService PermissionService)
        {
            this.MenuProjectMappingService = MenuProjectMappingService;
            this.MenuService = MenuService;
            this.PermissionService = PermissionService;
        }

        //get api/MenuProjectMappings
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<MenuProjectMapping>> response = new ResponseData<IEnumerable<MenuProjectMapping>>();
            try
            {
                IEnumerable<MenuProjectMapping> MenuProjectMapping = MenuProjectMappingService.GetMenuProjectMapping();
                response.SetData(MenuProjectMapping);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            //catch(DbUpdateException ex)
            //{
            //    response.SetStatus(HttpStatusCode.PreconditionFailed);
            //}
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("notdefault")]
        public HttpResponseMessage GetNotDefault()
        {
            ResponseData<IEnumerable<MenuDto>> response = new ResponseData<IEnumerable<MenuDto>>();
            try
            {
                IEnumerable<MenuDto> MenuProjectMapping = MenuProjectMappingService.GetMenuDtoDefault(0);
                response.SetData(MenuProjectMapping);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/MenuProjectMappings/id

        [HttpGet]
        [Route("{ProjectIDSys}")]
        public HttpResponseMessage Get(int ProjectIDSys)
        {
            IResponseData<List<MenuProjectMappingDto>> response = new ResponseData<List<MenuProjectMappingDto>>();
            try
            {
                List<MenuProjectMappingDto> MenuProjectMapping = MenuProjectMappingService.GetMenuProjectMappingByID(ProjectIDSys).ToList();
                response.SetData(MenuProjectMapping);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("menu/{ProjectIDSys}")]
        public HttpResponseMessage GetMenu(int ProjectIDSys)
        {

            IResponseData<List<MenuProjectMappingDto>> response = new ResponseData<List<MenuProjectMappingDto>>();
            try
            {
                GenMenu();
                List<MenuProjectMappingDto> menu = new List<MenuProjectMappingDto>(Menu);
                menu = Sorting(Menu);
                response.SetData(menu);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("default")]
        public HttpResponseMessage GetMenuDefault()
        {
            IResponseData<List<MenuDto>> response = new ResponseData<List<MenuDto>>();
            //IResponseData<IEnumerable<List<MenuProjectMappingDto>>> response2 = new ResponseData<IEnumerable<List<MenuProjectMappingDto>>>();
            try
            {
                IEnumerable<List<MenuDto>> MenuProjectMapping = MenuProjectMappingService.GetMenuDtoDefault(1).GroupBy(u => u.MenuParentID).Select(grp => grp.ToList());
                //response2.SetData(MenuProjectMapping);
                List<List<MenuDto>> temp = MenuProjectMapping.ToList();
                if (temp.Count != 0)
                {
                    List<MenuDto> MenuProjectMappingresponse = temp[0];
                    for (int i = 0; i < MenuProjectMappingresponse.Count<MenuDto>(); i++)
                    {
                        FindParents(MenuProjectMappingresponse[i], temp);
                    }

                    response.SetData(MenuProjectMappingresponse);
                }
                else
                { response.SetData(null); }
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/MenuProjectMappings/id/parent

        [HttpGet]
        [Route("parent/{ProjectIDSys}")]
        public HttpResponseMessage GetParent(int ProjectIDSys)
        {
            IResponseData<List<MenuProjectMappingDto>> response = new ResponseData<List<MenuProjectMappingDto>>();
            //IResponseData<IEnumerable<List<MenuProjectMappingDto>>> response2 = new ResponseData<IEnumerable<List<MenuProjectMappingDto>>>();
            try
            {
                IEnumerable<List<MenuProjectMappingDto>> MenuProjectMapping = MenuProjectMappingService.GetMenuProjectMappingByID(ProjectIDSys).GroupBy(u => u.MenuIDSysParent).OrderBy(u => u.Key).Select(grp => grp.ToList());
                List<List<Permission>> permission = PermissionService.GetPermissionByProjectID(ProjectIDSys).GroupBy(c => c.MenuIDSys).Select(grp => grp.ToList()).ToList();
                List<List<MenuProjectMappingDto>> temp = MenuProjectMapping.ToList();
                List<MenuProjectMappingDto> forfindPermission;
                for (int i = 0; i < temp.Count; i++)
                {
                    forfindPermission = temp[i];
                    for (int j = 0; j < temp[i].Count; j++)
                    {
                        forfindPermission[j].have = 0;
                        for (int k = 0; k < permission.Count; k++)
                        {
                            if (permission[k][0].MenuIDSys == forfindPermission[j].MenuIDSys)
                            {
                                forfindPermission[j].IsPermission = 1;
                                break;
                            }
                        }
                    }
                }

                List<MenuProjectMappingDto> MenuProjectMappingresponse = null;
                if (temp.Count > 0)
                {
                    MenuProjectMappingresponse = temp[0];
                    for (int i = 0; i < MenuProjectMappingresponse.Count<MenuProjectMappingDto>(); i++)
                    {
                        FindParent(MenuProjectMappingresponse[i], temp);
                    }
                }
                response.SetData(MenuProjectMappingresponse);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("parent")]
        public HttpResponseMessage GetParentProject()
        {
            IResponseData<List<MenuProjectMappingDto>> response = new ResponseData<List<MenuProjectMappingDto>>();
            //IResponseData<IEnumerable<List<MenuProjectMappingDto>>> response2 = new ResponseData<IEnumerable<List<MenuProjectMappingDto>>>();
            try
            {
                IEnumerable<List<MenuProjectMappingDto>> MenuProjectMapping = MenuProjectMappingService.GetMenuProjectMappingByID(User.Identity.GetProjectIDSys()).GroupBy(u => u.MenuIDSysParent).Select(grp => grp.ToList());
                List<Permission> permission = PermissionService.GetPermissionByProjectID(User.Identity.GetProjectIDSys());
                List<List<MenuProjectMappingDto>> temp = MenuProjectMapping.ToList();
                List<MenuProjectMappingDto> forfindPermission;
                for (int i = 0; i < temp.Count; i++)
                {
                    forfindPermission = temp[i];
                    for (int j = 0; j < temp[i].Count; j++)
                    {
                        forfindPermission[j].have = 0;
                        for (int k = 0; k < permission.Count; k++)
                        {
                            if (permission[k].MenuIDSys == forfindPermission[j].MenuIDSys)
                            {
                                forfindPermission[j].IsPermission = 1;
                            }
                        }
                    }
                }

                List<MenuProjectMappingDto> MenuProjectMappingresponse = null;
                if (temp.Count > 0)
                {
                    MenuProjectMappingresponse = temp[0];
                    for (int i = 0; i < MenuProjectMappingresponse.Count<MenuProjectMappingDto>(); i++)
                    {
                        FindParent(MenuProjectMappingresponse[i], temp);
                    }
                }
                response.SetData(MenuProjectMappingresponse);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Suppliers
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]MenuProjectMapping MenuProjectMapping)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {

                int id = MenuProjectMappingService.CreateMenuProjectMapping(MenuProjectMapping);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("{ProjectIDSys}")]
        public HttpResponseMessage PostMenu(List<MenuDto> menu, int ProjectIDSys)
        {
            IResponseData<int> response = new ResponseData<int>();
            byte forsort;
            for (int i = 0; i < menu.Count; i++)
            {
                menu[i].MenuParentID = 0;
                forsort = Convert.ToByte(i);
                // response.SetData(0);
                try
                {

                    if (menu[i].have == 1)
                    {
                        int id = MenuProjectMappingService.CreateMenuProjectMapping(menu[i], ProjectIDSys, forsort);
                        response.SetData(id);
                    }
                    if (menu[i].ParentMenu != null)
                    {
                        setParent(menu[i], ProjectIDSys);
                    }
                }
                catch (ValidationException ex)
                {
                    response.SetErrors(ex.Errors);
                    response.SetStatus(HttpStatusCode.PreconditionFailed);
                }
                catch (DbUpdateException ex)
                {
                    response.SetErrors(ex);
                    response.SetStatus(HttpStatusCode.PreconditionFailed);
                }
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        public void setParent(MenuDto mother, int ProjectID)
        {
            byte forsort;
            mother.have = 0;
            for (int j = 0; j < mother.ParentMenu.Count; j++)
            {
                mother.ParentMenu[j].MenuParentID = mother.MenuIDSys;
                mother.Sort = Convert.ToByte(j);
                forsort = Convert.ToByte(j);
                if (mother.ParentMenu[j].ParentMenu != null)
                {
                    setParent(mother.ParentMenu[j], ProjectID);
                }
                try
                {
                    if (mother.ParentMenu[j].have == 1)
                    {
                        int id = MenuProjectMappingService.CreateMenuProjectMapping(mother.ParentMenu[j], ProjectID, forsort);
                    }
                }
                catch (ValidationException)
                {
                }
            }
        }

        [HttpPut]
        [Route("")]
        public HttpResponseMessage PutLayout(List<MenuProjectMappingDto> MenuProjectMappingList)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            byte forsort;
            for (int i = 0; i < MenuProjectMappingList.Count; i++)
            {
                forsort = Convert.ToByte(i);
                MenuProjectMappingList[i].Sort = forsort;
                MenuProjectMappingList[i].MenuIDSysParent = 0;
                if (MenuProjectMappingList[i].ParentMenu != null)
                {
                    setParent(MenuProjectMappingList[i]);
                }
            }
            try
            {
                bool isUpated = MenuProjectMappingService.UpdateMenuProjectMapping(MenuProjectMappingList);
                response.SetData(true);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        public void GenMenu()
        {
            IEnumerable<MenuProjectMappingDto> MenuRes;
            Menu = new List<MenuProjectMappingDto>();
            int ProID = User.Identity.GetProjectIDSys();
            IEnumerable<MenuProjectMappingDto> res = new List<MenuProjectMappingDto>();
            using (CoreDbContext Db = new CoreDbContext())
            {
                if (User.IsSysAdmin())
                {
                    MenuPermis = MenuProjectMappingService.GetMenuProjectByID(ProID,Db);
                    var something = MenuPermis.ToList();
                    MenuRes = MenuPermis.Where(x => /*x.MenuIDSysParent != 0 &&*/ x.Url != null).GroupBy(x => x.MenuName).Select(grp => grp.First()).ToList();
                }
                else
                {
                    MenuPermis = MenuProjectMappingService.GetMenuProjectPermission(User.Identity.GetUserIdApp(), ProID,Db);
                    MenuRes = MenuPermis.GroupBy(x => x.MenuName).Select(grp => grp.First()).ToList();
                }
                IEnumerable<MenuProjectMappingDto> MenuAll = MenuProjectMappingService.GetAllMenu(ProID, MenuPermis.AsEnumerable(),Db).Select(row => new MenuProjectMappingDto
                {
                    MenuIDSys = row.MenuIDSys,
                    ProjectIDSys = row.ProjectIDSys,
                    MenuName = row.MenuName,
                    MenuIDSysParent = row.MenuIDSysParent,
                    Url = row.Url ?? String.Empty,
                    Sort = row.Sort
                }); ;
                //MenuRes = MenuProjectRes.Select(row => new MenuProjectMappingDto
                //{
                //    MenuIDSys = row.MenuIDSys,
                //    ProjectIDSys = row.ProjectIDSys,
                //    MenuName = row.MenuName,
                //    MenuIDSysParent = row.MenuIDSysParent,
                //    Url = row.Menu_MT.Url ?? String.Empty,
                //    Sort = row.Sort
                //});

                foreach (MenuProjectMappingDto resX in MenuRes)
                {
                    FindParentMenu(MenuAll, resX, resX.MenuIDSysParent);
                    if (MenuParent != null)
                    {
                        Menu.Add(MenuParent);
                        MenuParent = null;
                    }

                }
                List<MenuProjectMappingDto> resByZero = MenuRes.Where(x => x.MenuIDSysParent == 0 && x.Url != null).ToList();
                if (resByZero != null)
                {
                    foreach (var resp in resByZero)
                    {
                        Menu.Add(resp);
                    }
                }
            }
        }

        private bool FindChildenMenu(List<MenuProjectMappingDto> menu, MenuProjectMappingDto menuCur, int id)
        {
            try
            {
                MenuProjectMappingDto menux = menu.Where(x => x.MenuIDSys == id).FirstOrDefault();
                if (menux != null)
                {
                    List<MenuProjectMappingDto> ChkParentAv = menux.ParentMenu.Where(px => px.MenuIDSysParent == id && px.MenuIDSys == menuCur.MenuIDSys).ToList();
                    if (ChkParentAv.Count() == 0)
                    {
                        menux.ParentMenu.Add(menuCur);
                    }
                    else if (
                        ChkParentAv.Count() > 0
                        && ChkParentAv.FirstOrDefault().ParentMenu == null
                        && menuCur.ParentMenu.Count() > 0
                        )
                    {
                        menux.ParentMenu.FirstOrDefault().ParentMenu = new List<MenuProjectMappingDto>();
                        menux.ParentMenu.FirstOrDefault().ParentMenu.AddRange(menuCur.ParentMenu);
                    }
                    MenuParent = null;
                    return true;
                }
                else if (menu.FirstOrDefault() == null)
                {
                    return false;
                }
                else if (menu.Where(x => x.ParentMenu.Count() > 0).Count() > 0)
                {
                    List<MenuProjectMappingDto> menuxParent = menu.Where(x => x.ParentMenu.Count() > 0).ToList();
                    foreach (MenuProjectMappingDto menuDto in menuxParent)
                    {
                        List<MenuProjectMappingDto> menux2 = menuDto.ParentMenu;
                        FindChildenMenu(menux2, menuCur, id);
                    }

                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        private void FindParentMenu(IEnumerable<MenuProjectMappingDto> data, MenuProjectMappingDto menuCur, int id)
        {
            if (!FindChildenMenu(Menu, menuCur, id))
            {
                var munuall = from x in data
                              where x.MenuIDSys == id && x.ParentMenu == null
                              select x;
                MenuProjectMappingDto resX = munuall.FirstOrDefault();
                if (resX != null)
                {
                    if (resX.ParentMenu == null)
                    {
                        resX.ParentMenu = new List<MenuProjectMappingDto>();
                    }
                    MenuParent = resX;
                    MenuParent.ParentMenu.Add(menuCur);
                    if (resX.MenuIDSysParent != 0)
                    {
                        FindParentMenu(data, MenuParent, resX.MenuIDSysParent);
                    }
                }

            }
        }

        public void setParent(MenuProjectMappingDto mother)
        {
            byte forsort;
            for (int j = 0; j < mother.ParentMenu.Count; j++)
            {
                mother.ParentMenu[j].MenuIDSysParent = mother.MenuIDSys;
                mother.ParentMenu[j].Sort = Convert.ToByte(j);
                forsort = Convert.ToByte(j);
                if (mother.ParentMenu[j].ParentMenu != null)
                {
                    setParent(mother.ParentMenu[j]);
                }
            }
        }

        // PUT: api/Suppliers/5
        [HttpPut]
        [Route("{MenuProjectMappingIDSys}")]
        public HttpResponseMessage Put(int MenuProjectMappingIDSys, [FromBody]MenuProjectMapping MenuProjectMapping)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = MenuProjectMappingService.UpdateMenuProjectMapping(MenuProjectMapping);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpDelete]
        [Route("")]
        public HttpResponseMessage Delete(List<MenuProjectMappingDto> menu)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = MenuProjectMappingService.DeleteMenuProjectMapping(menu.ToList());
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        private IMenuProjectMappingService GetMenuProjectMappingService()
        {
            return MenuProjectMappingService;
        }

        public void FindParent2(MenuProjectMappingDto mother, List<List<MenuProjectMappingDto>> allData)
        {
            mother.have = 0;
            List<MenuProjectMappingDto> temp;
            List<MenuProjectMappingDto> permission = new List<MenuProjectMappingDto>();
            for (int i = 0; i < allData.Count(); i++)
            {
                //permission.Clear();
                temp = allData[i];
                if (temp[0].MenuIDSysParent == mother.MenuIDSys)
                {
                    for (int j = 0; j < temp.Count(); j++)
                    {
                        if (temp[j].IsPermission == Byte.Parse("1"))
                        {
                            permission.Add(temp[j]);
                        }
                    }
                    mother.ParentMenu = permission;
                }

            }
            if (mother.ParentMenu != null)
            {
                for (int i = 0; i < mother.ParentMenu.Count(); i++)
                {
                    FindParent(mother.ParentMenu[i], allData);
                }
            }
        }
        public void FindParent(MenuProjectMappingDto mother, List<List<MenuProjectMappingDto>> allData)
        {
            mother.have = 0;
            List<MenuProjectMappingDto> temp;
            for (int i = 0; i < allData.Count(); i++)
            {
                temp = allData[i];

                if (temp[0].MenuIDSysParent == mother.MenuIDSys)
                {
                    mother.ParentMenu = temp;
                }
            }
            if (mother.ParentMenu != null)
            {
                for (int i = 0; i < mother.ParentMenu.Count(); i++)
                {
                    FindParent(mother.ParentMenu[i], allData);
                }
            }
        }

        public void FindParents(MenuDto mother, List<List<MenuDto>> allData)
        {
            mother.have = 0;
            List<MenuDto> temp;
            for (int i = 0; i < allData.Count(); i++)
            {
                temp = allData[i];
                if (temp[0].MenuParentID == mother.MenuIDSys)
                {
                    mother.ParentMenu = temp;
                }
            }
            if (mother.ParentMenu != null)
            {
                for (int i = 0; i < mother.ParentMenu.Count(); i++)
                {
                    FindParents(mother.ParentMenu[i], allData);
                }
            }
        }

        public List<MenuProjectMappingDto> Sorting(List<MenuProjectMappingDto> data)
        {

            var change = data.OrderBy(x => x.Sort).ToList();
            data = change;
            foreach (var menu in data)
            {
                if (menu.ParentMenu != null)
                {
                    menu.ParentMenu = Sorting(menu.ParentMenu);
                }
            }
            return data;
        }

    }// end class

}

//[HttpGet]
//[Route("All")]
//public HttpResponseMessage GetMenuProjectMapping()
//{
//    IResponseData<List<MenuProjectMappingDto>> response = new ResponseData<List<MenuProjectMappingDto>>();
//    //IResponseData<IEnumerable<List<MenuProjectMappingDto>>> response2 = new ResponseData<IEnumerable<List<MenuProjectMappingDto>>>();
//    try
//    {
//        List<MenuProjectMappingDto> MenuProjectMapping = MenuProjectMappingService.GetMenuProjectMappingDto(1).ToList();
//        //response2.SetData(MenuProjectMapping);


//        response.SetData(MenuProjectMapping);
//    }
//    catch (ValidationException ex)
//    {
//        response.SetErrors(ex.Errors);
//        response.SetStatus(HttpStatusCode.PreconditionFailed);
//    }
//    return Request.ReturnHttpResponseMessage(response);
//}