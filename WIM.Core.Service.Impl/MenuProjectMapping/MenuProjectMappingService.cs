﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using System.Data.SqlClient;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Service;
using WIM.Core.Common.ValueObject;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Service.Impl
{
    public class MenuProjectMappingService : Service, IMenuProjectMappingService
    {

        public MenuProjectMappingService()
        {
        }

        public IEnumerable<MenuProjectMapping> GetMenuProjectMapping()
        {
            IEnumerable<MenuProjectMapping> menu;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                menu = repo.Get();
            }
            return menu.OrderBy(c => c.MenuName);
        }

        public int CreateMenuProjectMapping(MenuProjectMapping MenuProjectMapping)
        {
            using (var scope = new TransactionScope())
            {
                MenuProjectMapping data = new MenuProjectMapping();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                        data = repo.Insert(MenuProjectMapping);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }

                return data.MenuIDSys;
            }
        }

        public int CreateMenuProjectMapping(MenuDto MenuProjectMapping, int projectID, byte sort)
        {
            using (var scope = new TransactionScope())
            {
                MenuProjectMapping menuProjectMapping = new MenuProjectMapping();
                menuProjectMapping.MenuIDSys = MenuProjectMapping.MenuIDSys;
                menuProjectMapping.ProjectIDSys = projectID;
                menuProjectMapping.MenuName = MenuProjectMapping.MenuName;
                menuProjectMapping.MenuIDSysParent = MenuProjectMapping.MenuParentID;
                menuProjectMapping.Sort = sort;
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                        repo.Insert(menuProjectMapping);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return MenuProjectMapping.MenuIDSys;
            }
        }

        public bool UpdateMenuProjectMapping(MenuProjectMapping MenuProjectMapping)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                        repo.Update(MenuProjectMapping);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    MenuProjectMapping.MenuIDSys = 0;
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return true;
            }
        }

        public bool UpdateMenuProjectMapping(List<MenuProjectMappingDto> menuProjectMapping)
        {
            using (var scope = new TransactionScope())
            {

                try
                {

                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                        foreach (var c in menuProjectMapping)
                        {
                            MenuProjectMapping menu = new MenuProjectMapping();
                            menu.Sort = c.Sort;
                            menu.MenuIDSysParent = c.MenuIDSysParent;
                            menu.MenuName = c.MenuName;
                            menu.MenuIDSys = c.MenuIDSys;
                            menu.ProjectIDSys = c.ProjectIDSys;
                            repo.Update(menu);
                            if (c.ParentMenu != null)
                            {
                                setParent(c);
                            }
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return true;
            }
        }

        public void setParent(MenuProjectMappingDto mother)
        {
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                foreach (var c in mother.ParentMenu)
                {
                    MenuProjectMapping menu = new MenuProjectMapping();
                    menu.Sort = c.Sort;
                    menu.MenuIDSysParent = c.MenuIDSysParent;
                    menu.MenuName = c.MenuName;
                    menu.MenuIDSys = c.MenuIDSys;
                    menu.ProjectIDSys = c.ProjectIDSys;
                    repo.Update(menu);
                    if (c.ParentMenu != null)
                    {
                        setParent(c);
                    }
                }
                Db.SaveChanges();
            }
        }


        public bool DeleteMenuProjectMapping(List<MenuProjectMappingDto> menu)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                        for (int i = 0; i < menu.Count; i++)
                        {
                            MenuProjectMapping x = new MenuProjectMapping();
                            x.MenuIDSys = menu[i].MenuIDSys;
                            x.ProjectIDSys = menu[i].ProjectIDSys;
                            repo.Delete(x);
                            if (menu[i].ParentMenu != null)
                            {
                                DeleteMenuProjectMapping(menu[i].ParentMenu);
                            }
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                    throw ex;
                }


                return true;
            }
        }

        public IEnumerable<MenuProjectMappingDto> GetMenuProjectMappingByID(int id)
        {
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                string[] include = { "Menu_MT" };
                MenuProjectMappingdto = repo.GetWithInclude((c => c.ProjectIDSys == id), include).OrderBy(c => c.MenuIDSysParent)
               .OrderBy(c => c.Sort).Select(b => new MenuProjectMappingDto()
               {
                   MenuIDSys = b.MenuIDSys,
                   ProjectIDSys = b.ProjectIDSys,
                   MenuName = b.MenuName,
                   MenuIDSysParent = b.MenuIDSysParent,
                   Url = b.Menu_MT.Url,
                   Sort = b.Sort
               }).ToList();
            }
            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuDto> GetMenuDtoByProjectID(int id)
        {
            IEnumerable<MenuDto> MenuProjectMappingdto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                MenuProjectMappingdto = repo.GetMany((c => c.ProjectIDSys == id)).OrderBy(c => c.MenuIDSysParent)
               .OrderBy(c => c.Sort).Select(b => new MenuDto()
               {
                   MenuIDSys = b.MenuIDSys,
                   MenuName = b.MenuName,
                   MenuParentID = b.MenuIDSysParent,
                   Sort = b.Sort,
                   Url = b.Menu_MT.Url,
                   IsPermission = 0
               }).ToList();
            }
            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuProjectMappingDto> GetAllMenu(int projectid, IEnumerable<MenuProjectMappingDto> menu)
        {
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                var MenuProjectMappingQuery = repo.GetAllMenu(projectid, menu);
                MenuProjectMappingdto = MenuProjectMappingQuery;
            }
            return MenuProjectMappingdto;

        }

        public IEnumerable<MenuProjectMappingDto> GetMenuPermission(string userid, int projectid)
        {
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                var menu = repo.GetMenuPermission(userid, projectid);

                //var menu = db.MenuProjectMappings.SqlQuery(
                //    "SELECT e.MenuIDSys,e.ProjectIDSys,e.MenuIDSysParent,e.Sort,e.MenuName,e.MenuPic FROM UserRoles a " +
                //    " INNER JOIN RolePermission b ON a.RoleID = b.RoleID" +
                //    " INNER JOIN Roles d ON a.RoleID = d.RoleID" +
                //    " INNER JOIN Permissions c ON b.PermissionID = c.PermissionID" +
                //    " INNER JOIN MenuProjectMapping e ON c.MenuIDSys = e.MenuIDSys" +
                //    " INNER JOIN Menu_MT f ON e.MenuIDSys = f.MenuIDSys" +
                //    " WHERE d.ProjectIDSys = {0} AND a.UserID = {1} AND e.ProjectIDSys = {0}"
                //    , projectid, userid).ToList();

                MenuProjectMappingdto = menu.Select(b => new MenuProjectMappingDto()
                {
                    MenuIDSys = b.MenuIDSys,
                    ProjectIDSys = b.ProjectIDSys,
                    MenuName = b.MenuName,
                    MenuIDSysParent = b.MenuIDSysParent,
                    Url = b.Menu_MT.Url,
                    Sort = b.Sort
                }).ToList();
            }
            return MenuProjectMappingdto;

        }

        public IEnumerable<MenuDto> GetMenuDtoDefault(int i)
        {
            IEnumerable<MenuDto> MenuProjectMappingdto;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IMenuRepository repo = new MenuRepository(Db);
                MenuProjectMappingdto = repo.GetMany((c => c.IsDefault == i)).Select(b =>
            new MenuDto()
            {
                MenuIDSys = b.MenuIDSys,
                MenuName = b.MenuName,
                MenuParentID = b.MenuParentID,
                Sort = b.Sort,
                Url = b.Url,
                Api = b.Api,
                have = 1
            }
                ).ToList();
            }

            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuProjectMappingDto> GetMenuProjectByID(int id, CoreDbContext x)
        {
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto;
            IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(x);
            var something = repo.GetMenuProject(id).ToList();
            MenuProjectMappingdto = repo.GetMenuProject(id)
           .Select(b => new MenuProjectMappingDto()
           {
               MenuIDSys = b.MenuIDSys,
               ProjectIDSys = b.ProjectIDSys,
               MenuName = b.MenuName,
               MenuIDSysParent = b.MenuIDSysParent,
               Url = b.Menu_MT.Url,
               Sort = b.Sort
           });
            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuProjectMappingDto> GetMenuProjectPermission(string userid, int projectid, CoreDbContext x)
        {
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto;
            CoreDbContext Db = x;
            IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
            var menu = repo.GetMenuPermission(userid, projectid);
            MenuProjectMappingdto = menu.Select(b => new MenuProjectMappingDto()
            {
                MenuIDSys = b.MenuIDSys,
                ProjectIDSys = b.ProjectIDSys,
                MenuName = b.MenuName,
                MenuIDSysParent = b.MenuIDSysParent,
                Url = b.Menu_MT.Url,
                Sort = b.Sort
            }
               );
            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuProjectMappingDto> GetAllMenu(int projectid, IEnumerable<MenuProjectMappingDto> menu,CoreDbContext x)
        {
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto;
            using (CoreDbContext Db = x)
            {
                IMenuProjectMappingRepository repo = new MenuProjectMappingRepository(Db);
                var MenuProjectMappingQuery = repo.GetAllMenuWithContext(projectid, menu,x);
                MenuProjectMappingdto = MenuProjectMappingQuery.ToList();
            }
            return MenuProjectMappingdto;
        }

    }
}
