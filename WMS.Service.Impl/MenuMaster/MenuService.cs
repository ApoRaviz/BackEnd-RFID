using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class MenuService : IMenuService
    {
        private MenuRepository repo;

        public MenuService()
        {
            repo = new MenuRepository();
        }

        public IEnumerable<Menu_MT> GetMenu()
        {
            return repo.Get();
        }

        public Menu_MT GetMenuByMenuIDSys(int id)
        {
            Menu_MT Menu = repo.GetByID(id);
            return Menu;
        }

        public int CreateMenu(Menu_MT Menu)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Insert(Menu);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException )
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return Menu.MenuIDSys;
            }
        }

        public int CreateMenu(MenuDto Menu,int projectID,byte sort)
        {
            using (var scope = new TransactionScope())
            {
                Menu_MT menu = new Menu_MT();
                menu.MenuName = Menu.MenuName;
                menu.MenuParentID = Menu.MenuParentID;
                menu.Url = Menu.Url;
                menu.Api = Menu.Api;
                menu.Sort = sort;
                menu.ProjectIDSys = projectID;
                menu.MenuPic = Menu.Icon;
                try
                {
                    repo.Insert(menu);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                
                return menu.MenuIDSys;
            }
        }
  

        public bool UpdateMenu(int id, Menu_MT Menu)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Update(Menu);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return true;
            }
        }

        public bool UpdateMenu(List<MenuDto> menu)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    
                foreach (var c in menu)
                {   Menu_MT existedMenu;
                    existedMenu = repo.GetByID(c.MenuIDSys);
                    existedMenu.MenuParentID = c.MenuParentID;
                    existedMenu.MenuName = c.MenuName;
                    existedMenu.Sort = c.Sort;
                    repo.Update(existedMenu);
                }
                
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                
                return true;
            }
        }

        public bool DeleteMenu(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedMenu = repo.GetByID(id);
                repo.Update(existedMenu);
                try
                {
                scope.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }


                return true;
            }
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

        public IEnumerable<MenuDto> GetMenuByMenuParentID(int id)
        {
            IEnumerable<MenuDto> menudto = repo.GetByMenuParentID(id).Select(b =>
            new MenuDto()
            {
                MenuIDSys = b.MenuIDSys,
                MenuName = b.MenuName,
                MenuParentID = b.MenuParentID,
                Url = b.Url,
                Api = b.Api,
                Sort = b.Sort,
                Icon = b.MenuPic
            }
                ).ToList();

            return menudto;
        }

        public IEnumerable<MenuDto> GetMenuDto()
        {
            var menuQuery = repo.Get();
            Console.Write(menuQuery);
            IEnumerable<MenuDto> menudto = menuQuery.Select(b =>
            new MenuDto()
            {
                MenuIDSys = b.MenuIDSys,
                MenuName = b.MenuName,
                MenuParentID = b.MenuParentID,
                Url = b.Url,
                Api = b.Api,
                Sort = b.Sort,
                Icon = b.MenuPic
            }
                ).ToList();
            return menudto;
        }
        public IEnumerable<MenuDto> GetMenuDto(int projectIDSys)
        {
            var menuQuery = repo.Get();
            Console.Write(menuQuery);
            IEnumerable<MenuDto> menudto = menuQuery.Select(b =>
            new MenuDto()
            {
                MenuIDSys = b.MenuIDSys,
                MenuName = b.MenuName,
                MenuParentID = b.MenuParentID,
                Url = b.Url,
                Api = b.Api,
                Sort = b.Sort,
                Icon = b.MenuPic
            }
                ).ToList();
            return menudto;
        }
        public IEnumerable<MenuDto> GetMenuDtoNotHave(int projectIDSys)
        {
            var menuQuery = repo.GetNotHave(projectIDSys);
            Console.Write(menuQuery);
            IEnumerable<MenuDto> menudto = menuQuery.Select(b =>
            new MenuDto()
            {
                MenuIDSys = b.MenuIDSys,
                MenuName = b.MenuName,
                MenuParentID = b.MenuParentID,
                Url = b.Url,
                Api = b.Api,
                Sort = b.Sort,
                Icon = b.MenuPic,
                have = 1
            }
                ).ToList();
            return menudto;
        }
    }
}
