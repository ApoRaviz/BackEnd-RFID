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
using System.Data.SqlClient;
using WMS.Common;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Security.Context;

namespace WMS.Service
{
    public class MenuProjectMappingService : IMenuProjectMappingService
    {
        private CoreDbContext db = CoreDbContext.Create();
        private SecurityDbContext SecuDb;
        private GenericRepository<MenuProjectMapping> repo;

        public MenuProjectMappingService()
        {
            SecuDb = new SecurityDbContext();
            repo = new GenericRepository<MenuProjectMapping>(db);
        }

        public IEnumerable<MenuProjectMapping> GetMenuProjectMapping()
        {
            return repo.GetAll();
        }

        public MenuProjectMapping GetMenuProjectMappingByMenuProjectMappingIDSys(int id)
        {
            MenuProjectMapping MenuProjectMapping = db.MenuProjectMapping.Find(id);
            return MenuProjectMapping;
        }

        public int CreateMenuProjectMapping(MenuProjectMapping MenuProjectMapping)
        {
            using (var scope = new TransactionScope())
            {

                repo.Insert(MenuProjectMapping);
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
                return MenuProjectMapping.MenuIDSys;
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
                repo.Insert(menuProjectMapping);
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

                return MenuProjectMapping.MenuIDSys;
            }
        }

        public bool UpdateMenuProjectMapping(int id, MenuProjectMapping MenuProjectMapping)
        {
            using (var scope = new TransactionScope())
            {
                var existedMenuProjectMapping = repo.GetByID(id);
                existedMenuProjectMapping.Sort = MenuProjectMapping.Sort;
                existedMenuProjectMapping.MenuIDSysParent = MenuProjectMapping.MenuIDSysParent;
                existedMenuProjectMapping.MenuName = MenuProjectMapping.MenuName;
                //existedMenuProjectMapping.Sort = MenuProjectMapping.Sort;
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
                    MenuProjectMapping.MenuIDSys = 0;
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return true;
            }
        }

        public bool UpdateMenuProjectMapping(List<MenuProjectMappingDto> menuProjectMapping)
        {
            using (var scope = new TransactionScope())
            {
                
                
                foreach(var c in menuProjectMapping)
                {
                    MenuProjectMapping menu = new MenuProjectMapping();
                    menu.Sort = c.Sort;
                    menu.MenuIDSysParent = c.MenuIDSysParent;
                    menu.MenuName = c.MenuName;
                    menu.MenuIDSys = c.MenuIDSys;
                    menu.ProjectIDSys = c.ProjectIDSys;
                    repo.Update(menu);
                    if(c.ParentMenu != null)
                    {
                        setParent(c);
                    }
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
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }

                
                return true;
            }
        }

        public void setParent(MenuProjectMappingDto mother)
        {
            foreach (var c in mother.ParentMenu)
            {
                MenuProjectMapping menu = new MenuProjectMapping();
                menu.Sort = c.Sort;
                menu.MenuIDSysParent = c.MenuIDSysParent;
                menu.MenuName = c.MenuName;
                menu.MenuIDSys = c.MenuIDSys;
                menu.ProjectIDSys = c.ProjectIDSys;
                repo.Update(menu);
                if(c.ParentMenu != null)
                {
                    setParent(c);
                }
            }
        }


        public bool DeleteMenuProjectMapping(List<MenuProjectMappingDto> menu)
        {
            using (var scope = new TransactionScope())
            {

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
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }

                scope.Complete();
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

        public void HandleUpdateException(DbUpdateException ex)
        {
            throw new DbUpdateException(ex.Message);

        }


        public void HandleUpdateException(ValidationException ex)
        {
            throw ex;

        }

        public IEnumerable<MenuProjectMappingDto> GetMenuProjectMappingByID(int id)
        {
            var tbl_MenuProjectMapping = db.MenuProjectMapping;
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto = tbl_MenuProjectMapping.Where(t => t.ProjectIDSys == id).Select(b =>
            new MenuProjectMappingDto()
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

        public IEnumerable<MenuDto> GetMenuDtoByProjectID(int id)
        {
            var tbl_MenuProjectMapping = db.MenuProjectMapping;
            IEnumerable<MenuDto> MenuProjectMappingdto = tbl_MenuProjectMapping.Where(t => t.ProjectIDSys == id).Select(b =>
            new MenuDto()
            {
                MenuIDSys = b.MenuIDSys,
                MenuName = b.MenuName,
                MenuParentID = b.MenuIDSysParent,
                Sort = b.Sort,
                Url = b.Menu_MT.Url,
                IsPermission = 0
            }
                ).ToList();

            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuProjectMappingDto> GetMenuProjectMappingDto(int id)
        {
            var MenuProjectMappingQuery = from row in db.MenuProjectMapping
                                          where row.ProjectIDSys == id
                                          orderby row.MenuIDSysParent, row.Sort
                                          select row;

            var tbl_MenuProjectMapping = db.MenuProjectMapping;
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto = MenuProjectMappingQuery.Select(b =>
            new MenuProjectMappingDto()
            {
                MenuIDSys = b.MenuIDSys,
                ProjectIDSys = b.ProjectIDSys,
                MenuName = b.MenuName,
                MenuIDSysParent = b.MenuIDSysParent,
                Url = b.Menu_MT.Url,
                Sort = b.Sort
            }
                ).OrderBy(b => b.Sort).ToList();
            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuProjectMappingDto> GetAllMenu(int projectid, IEnumerable<MenuProjectMappingDto> menu)
        {
            var MenuProjectMappingQuery = (from row in db.MenuProjectMapping
                                           join o in menu on row.MenuIDSys equals o.MenuIDSys into joined
                                           from i in joined.DefaultIfEmpty()
                                           where row.ProjectIDSys == projectid
                                           orderby row.MenuIDSysParent, row.Sort
                                           select new
                                           {
                                               MenuIDSys = row.MenuIDSys,
                                               ProjectIDSys = row.ProjectIDSys,
                                               MenuName = row.MenuName,
                                               MenuIDSysParent = row.MenuIDSysParent,
                                               Url = i.Url ?? String.Empty,
                                               Sort = row.Sort
                                           });
            var tbl_MenuProjectMapping = db.MenuProjectMapping;
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto = MenuProjectMappingQuery.Select(b =>
            new MenuProjectMappingDto()
            {
                MenuIDSys = b.MenuIDSys,
                ProjectIDSys = b.ProjectIDSys,
                MenuName = b.MenuName,
                MenuIDSysParent = b.MenuIDSysParent,
                Url = b.Url,
                Sort = b.Sort
            }
                );
            return MenuProjectMappingdto;

        }

        public IEnumerable<MenuProjectMappingDto> GetMenuPermission(string userid, int projectid)
        {
            var menu = from ur in db.UserRoles
                       join rp in db.RolePermission on ur.RoleID equals rp.RoleID
                       join ps in db.Permission on rp.PermissionID equals ps.PermissionID
                       join r in db.Role on ur.RoleID equals r.RoleID
                       join mp in db.MenuProjectMapping on ps.MenuIDSys equals mp.MenuIDSys
                       where r.ProjectIDSys == projectid && ur.UserID == userid && mp.ProjectIDSys == projectid
                       select mp;



            //var menu = db.MenuProjectMappings.SqlQuery(
            //    "SELECT e.MenuIDSys,e.ProjectIDSys,e.MenuIDSysParent,e.Sort,e.MenuName,e.MenuPic FROM UserRoles a " +
            //    " INNER JOIN RolePermission b ON a.RoleID = b.RoleID" +
            //    " INNER JOIN Roles d ON a.RoleID = d.RoleID" +
            //    " INNER JOIN Permissions c ON b.PermissionID = c.PermissionID" +
            //    " INNER JOIN MenuProjectMapping e ON c.MenuIDSys = e.MenuIDSys" +
            //    " INNER JOIN Menu_MT f ON e.MenuIDSys = f.MenuIDSys" +
            //    " WHERE d.ProjectIDSys = {0} AND a.UserID = {1} AND e.ProjectIDSys = {0}"
            //    , projectid, userid).ToList();

            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto = menu.Select(b =>
            new MenuProjectMappingDto()
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

        public IEnumerable<MenuDto> GetMenuDtoDefault(int i)
        {
            var tbl_MenuProjectMapping = db.Menu_MT;
            IEnumerable<MenuDto> MenuProjectMappingdto = tbl_MenuProjectMapping.Where(t => t.IsDefault == i).Select(b =>
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

            return MenuProjectMappingdto;
        }

    }
}
