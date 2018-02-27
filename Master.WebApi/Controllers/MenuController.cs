
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Service;

namespace Master.WebApi.Controllers
{
    // [Authorize]
    [RoutePrefix("api/v1/Menus")]
    public class MenuController : ApiController
    {
        private IMenuService MenuService;

        public MenuController(IMenuService MenuService)
        {
            this.MenuService = MenuService;
        }

        //get api/Menus
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            IResponseData<List<MenuDto>> response = new ResponseData<List<MenuDto>>();
            try
            {
                List<MenuDto> Menu = MenuService.GetMenuDto().OrderBy(a => a.MenuName).ToList();
                //response2.SetData(Menu);


                response.SetData(Menu);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/Menus/id
        [HttpGet]
        [Route("{MenuIDSys}")]
        public HttpResponseMessage Get(int MenuIDSys)
        {
            IResponseData<Menu_MT> response = new ResponseData<Menu_MT>();
            try
            {
                Menu_MT Menu = MenuService.GetMenuByMenuIDSys(MenuIDSys);
                response.SetData(Menu);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/Menus/id/parent
        [HttpGet]
        [Route("parent")]
        public HttpResponseMessage GetParent()
        {
            IResponseData<List<MenuDto>> response = new ResponseData<List<MenuDto>>();
            //IResponseData<IEnumerable<List<MenuDto>>> response2 = new ResponseData<IEnumerable<List<MenuDto>>>();
            try
            {
                IEnumerable<List<MenuDto>> Menu = MenuService.GetMenuDto().GroupBy(u => u.MenuParentID).Select(grp => grp.ToList());
                //response2.SetData(Menu);
                List<List<MenuDto>> temp = Menu.ToList();
                List<MenuDto> menuresponse = temp[0];
                for (int i = 0; i < menuresponse.Count<MenuDto>(); i++)
                {
                    FindParent(menuresponse[i], temp);
                }

                response.SetData(menuresponse);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("autocomplete/{term}")]
        public HttpResponseMessage AutocompleteCustomer(string term)
        {
            IResponseData<IEnumerable<AutocompleteMenuDto>> response = new ResponseData<IEnumerable<AutocompleteMenuDto>>();
            try
            {
                if (string.IsNullOrEmpty(term))
                {
                    throw new Exception("Missing term");
                }
                IEnumerable<AutocompleteMenuDto> menu = MenuService.AutocompleteMenu(term);
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
        [Route("project/{ProjectIDSys}")]
        public HttpResponseMessage GetMenuNotinProject(int ProjectIDSys)
        {
            IResponseData<List<MenuDto>> response = new ResponseData<List<MenuDto>>();
            //IResponseData<IEnumerable<List<MenuDto>>> response2 = new ResponseData<IEnumerable<List<MenuDto>>>();
            try
            {
                //List<List<MenuDto>> Menu = MenuService.GetMenuDtoNotHave(ProjectIDSys).GroupBy(b => b.MenuParentID).Select(grp => grp.ToList()).ToList();
                List<MenuDto> Menu = MenuService.GetMenuDtoNotHave(ProjectIDSys).ToList();
                List<MenuDto> Menutemp = new List<MenuDto>(Menu);
                List<MenuDto> menu = new List<MenuDto>();
                foreach (var x in Menu)
                {
                    FindParent(x, Menu, Menutemp);
                }
                foreach(var x in Menu)
                {
                    if (Menutemp.Any(y => y.MenuIDSys == x.MenuIDSys))
                    {
                        menu.Add(x);
                    }
                }
                //List<MenuDto> menu = Menu[0];
                //List<MenuDto> menu2 = new List<MenuDto>();
                //for (int i = 0; i < menu.Count<MenuDto>(); i++)
                //{
                //    FindParent(menu[i], Menu);
                //}
                response.SetData(menu);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        public void FindParent(MenuDto mother, List<List<MenuDto>> allData)
        {
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
                    FindParent(mother.ParentMenu[i], allData);
                }
            }
        }

        public void setParent(MenuDto mother)
        {
            byte forsort;
            for (int j = 0; j < mother.ParentMenu.Count; j++)
            {
                mother.ParentMenu[j].MenuParentID = mother.MenuIDSys;
                mother.Sort = Convert.ToByte(j);
                forsort = Convert.ToByte(j);
                if (mother.ParentMenu[j].ParentMenu != null)
                {
                    setParent(mother.ParentMenu[j]);
                }
                try
                {
                    mother.ParentMenu[j].MenuParentID = mother.ParentMenu[j].MenuIDSys;
                    mother.ParentMenu[j].Sort = forsort;
                }
                catch (ValidationException)
                {
                }
            }
        }


        // POST: api/Suppliers
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Menu_MT Menu)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {

                int id = MenuService.CreateMenu(Menu);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }




        [HttpPut]
        [Route("")]
        public HttpResponseMessage PutLayout(List<MenuDto> MenuList)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            byte forsort;
            for (int i = 0; i < MenuList.Count; i++)
            {
                MenuList[i].MenuParentID = 0;
                forsort = Convert.ToByte(i);
                if (MenuList[i].ParentMenu != null)
                {
                    setParent(MenuList[i]);
                    MenuList[i].MenuParentID = MenuList[i].MenuIDSys;
                    MenuList[i].Sort = forsort;
                }
            }
            try
            {
                bool isUpated = MenuService.UpdateMenu(MenuList);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Suppliers/5
        [HttpPut]
        [Route("{MenuIDSys}")]
        public HttpResponseMessage Put(int MenuIDSys, [FromBody]Menu_MT Menu)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = MenuService.UpdateMenu(Menu);
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
        [Route("{MenuIDSys}")]
        public HttpResponseMessage Delete(int MenuIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = MenuService.DeleteMenu(MenuIDSys);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        private IMenuService GetMenuService()
        {
            return MenuService;
        }

        public void setParent(MenuDto mother, int ProjectID)
        {
            byte forsort;
            for (int j = 0; j < mother.ParentMenu.Count; j++)
            {
                mother.ParentMenu[j].MenuParentID = mother.MenuIDSys;
                mother.Sort = Convert.ToByte(j);
                forsort = Convert.ToByte(j);
                if (mother.ParentMenu[j].ParentMenu != null)
                {
                    setParent(mother.ParentMenu[j]);
                }
                try
                {
                    int id = MenuService.CreateMenu(mother.ParentMenu[j], ProjectID, forsort);

                }
                catch (ValidationException)
                {
                }
            }
        }

        public void FindParent(MenuDto mother, List<MenuDto> allData , List<MenuDto> Menuforcheck)
        {
            MenuDto temp;
            if(allData.Any(x => x.MenuParentID == mother.MenuIDSys))
            {
                var p = allData.Where(x => x.MenuParentID == mother.MenuIDSys).ToList();
            
            for (int i = 0; i < p.Count(); i++)
            {
                temp = p[i];
                if (temp.MenuParentID == mother.MenuIDSys)
                {
                    if(mother.ParentMenu != null)
                    {
                        if(!mother.ParentMenu.Any(y => y.MenuIDSys == temp.MenuIDSys))
                        mother.ParentMenu.Add(temp);
                    }
                    else
                    {
                        mother.ParentMenu = new List<MenuDto>();
                        mother.ParentMenu.Add(temp);
                    }
                    
                    Menuforcheck.Remove(temp);
                }
            }
            if (mother.ParentMenu != null)
            {
                for (int i = 0; i < mother.ParentMenu.Count(); i++)
                {
                    FindParent(mother.ParentMenu[i], allData , Menuforcheck);
                }
            }

            }
        }

    }//end class
}