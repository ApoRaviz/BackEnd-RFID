﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;

namespace WIM.Core.Service.Impl
{
    public class MenuService : IMenuService
    {
        private IIdentity user { get; set; }
        public MenuService(IIdentity identity)
        {
            user = identity;
        }

        public IEnumerable<Menu_MT> GetMenu()
        {
            IEnumerable<Menu_MT> menu;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuRepository repo = new MenuRepository(Db);
                menu = repo.Get().OrderBy(c => c.MenuParentID).OrderBy(c => c.Sort);
            }
            return menu;
        }

        public Menu_MT GetMenuByMenuIDSys(int id)
        {
            Menu_MT Menu;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuRepository repo = new MenuRepository(Db);
                Menu = repo.GetByID(id);
            }
            return Menu;
        }

        public int CreateMenu(Menu_MT Menu)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuRepository repo = new MenuRepository(Db);
                        repo.Insert(Menu);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
                return Menu.MenuIDSys;
            }
        }

        public int CreateMenu(MenuDto Menu, int projectID, byte sort)
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
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuRepository repo = new MenuRepository(Db);
                        repo.Insert(menu);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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


        public bool UpdateMenu(Menu_MT Menu)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuRepository repo = new MenuRepository(Db);
                        repo.Update(Menu);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuRepository repo = new MenuRepository(Db);

                        foreach (var c in menu)
                        {
                            Menu_MT existedMenu;
                            existedMenu = repo.GetByID(c.MenuIDSys);
                            existedMenu.MenuParentID = c.MenuParentID;
                            existedMenu.MenuName = c.MenuName;
                            existedMenu.Sort = c.Sort;
                            repo.Update(existedMenu);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuRepository repo = new MenuRepository(Db);
                        var existedMenu = repo.GetByID(id);
                        repo.Delete(existedMenu);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
            IEnumerable<MenuDto> menudto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuRepository repo = new MenuRepository(Db);
                menudto = repo.GetMany(c => c.MenuParentID == id).Select(b =>
                new MenuDto()
                {
                         MenuIDSys = b.MenuIDSys,
                         MenuName = b.MenuName,
                         MenuParentID = b.MenuParentID,
                         Url = b.Url,
                         Api = b.Api,
                         Sort = b.Sort,
                         Icon = b.MenuPic
                }).ToList();
            }
            return menudto;
        }

        public IEnumerable<MenuDto> GetMenuDto()
        {
            IEnumerable<MenuDto> menudto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuRepository repo = new MenuRepository(Db);
                var menuQuery = repo.Get();
                Console.Write(menuQuery);
                menudto = menuQuery.Select(b =>
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
            }
            return menudto;
        }
        public IEnumerable<MenuDto> GetMenuDto(int projectIDSys)
        {
            IEnumerable<MenuDto> menudto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuRepository repo = new MenuRepository(Db);
                var menuQuery = repo.Get().OrderBy(c => c.MenuParentID).OrderBy(c => c.Sort);
                Console.Write(menuQuery);
                menudto = menuQuery.Select(b =>
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
            }
            return menudto;
        }
        public IEnumerable<MenuDto> GetMenuDtoNotHave(int projectIDSys)
        {
            IEnumerable<MenuDto> menudto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuRepository repo = new MenuRepository(Db);
                var menuQuery = repo.GetMany((c => !(Db.MenuProjectMapping).Where(a => a.ProjectIDSys == projectIDSys)
                .Select(a=>a.MenuIDSys).Contains(c.MenuIDSys))).OrderBy(c => c.MenuParentID).OrderBy(c => c.Sort);
                Console.Write(menuQuery);
                menudto = menuQuery.Select(b => new MenuDto()
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
            }
            return menudto;
        }
    }
}
