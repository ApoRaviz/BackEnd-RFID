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
using WMS.Master.Menu;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master
{
    public class MenuService : IMenuService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Menu_MT> repo;

        public MenuService()
        {
            repo = new GenericRepository<Menu_MT>(db);
        }

        public IEnumerable<Menu_MT> GetMenu()
        {
            return repo.GetAll();
        }



        public Menu_MT GetMenuByMenuIDSys(int id)
        {
            Menu_MT Menu = db.Menu_MT.Find(id);
            return Menu;
        }

        public int CreateMenu(Menu_MT Menu)
        {
            using (var scope = new TransactionScope())
            {

                repo.Insert(Menu);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
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
                repo.Insert(menu);
                try
                {
                    db.SaveChanges();
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
                var existedMenu = repo.GetByID(id);
                existedMenu.MenuParentID = Menu.MenuParentID;
                existedMenu.MenuName = Menu.MenuName;
                existedMenu.Url = Menu.Url;
                existedMenu.Api = Menu.Api;
                existedMenu.Sort = Menu.Sort;
                try
                {
                    db.SaveChanges();
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
                scope.Complete();
                return true;
            }
        }

        public bool UpdateMenu(List<MenuDto> menu)
        {
            using (var scope = new TransactionScope())
            {
                Menu_MT existedMenu;
                foreach (var c in menu)
                {
                    existedMenu = repo.GetByID(c.MenuIDSys);
                    existedMenu.MenuParentID = c.MenuParentID;
                    existedMenu.MenuName = c.MenuName;
                    existedMenu.Sort = c.Sort;
                }
                try
                {
                    db.SaveChanges();
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
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
                db.SaveChanges();
                scope.Complete();
                }
                catch (DbUpdateConcurrencyException e)
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
            var tbl_menu = db.Menu_MT;
            IEnumerable<MenuDto> menudto = tbl_menu.Where(t => t.MenuParentID == id).Select(b =>
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
            var menuQuery = from row in db.Menu_MT
                                 orderby row.MenuParentID , row.Sort
                                 select row;
            Console.Write(menuQuery);
            var tbl_menu = db.Menu_MT;
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
            var menuQuery = from row in db.Menu_MT
                            orderby row.MenuParentID, row.Sort
                            select row;
            Console.Write(menuQuery);
            var tbl_menu = db.Menu_MT;
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
            var menuQuery = from row in db.Menu_MT
                            where !(from o in db.MenuProjectMappings
                                    where o.ProjectIDSys == projectIDSys
                                    select o.MenuIDSys)
                                    .Contains(row.MenuIDSys)
                            orderby row.MenuParentID, row.Sort
                            select row;
            Console.Write(menuQuery);
            var tbl_menu = db.Menu_MT;
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
