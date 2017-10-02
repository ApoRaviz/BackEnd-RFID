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
using WMS.Master;
using System.Diagnostics;
using WMS.Master.Menu;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master
{
    public class MenuProjectMappingService : IMenuProjectMappingService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<MenuProjectMapping> repo;

        public MenuProjectMappingService()
        {
            repo = new GenericRepository<MenuProjectMapping>(db);
        }

        public IEnumerable<MenuProjectMapping> GetMenuProjectMapping()
        {
            return repo.GetAll();
        }



        public MenuProjectMapping GetMenuProjectMappingByMenuProjectMappingIDSys(int id)
        {
            MenuProjectMapping MenuProjectMapping = db.MenuProjectMappings.Find(id);
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
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return MenuProjectMapping.MenuIDSys;
            }
        }

        public int CreateMenuProjectMapping(MenuDto MenuProjectMapping,int projectID,byte sort)
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
                catch (DbUpdateException e)
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
                catch (DbUpdateException e)
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

        public bool UpdateMenuProjectMapping( MenuProjectMappingDto menuProjectMapping , byte sort)
        {
            using (var scope = new TransactionScope())
            {
                MenuProjectMapping menu = new MenuProjectMapping();
                menu.Sort = sort;
                menu.MenuIDSysParent = menuProjectMapping.MenuIDSysParent;
                menu.MenuName = menuProjectMapping.MenuName;
                menu.MenuIDSys = menuProjectMapping.MenuIDSys;
                menu.ProjectIDSys = menuProjectMapping.ProjectIDSys;
                //existedMenuProjectMapping.Sort = MenuProjectMapping.Sort;
                repo.Update(menu);
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
                return true;
            }
        }


        public bool DeleteMenuProjectMapping(List<MenuProjectMappingDto> menu)
        {
            using (var scope = new TransactionScope())
            {
                
                for (int i = 0; i< menu.Count; i++)
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
                }catch(DbUpdateConcurrencyException e)
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
            throw  ex;

        }

        public IEnumerable<MenuProjectMappingDto> GetMenuProjectMappingByID(int id)
        {
            var tbl_MenuProjectMapping = db.MenuProjectMappings;
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
                ).ToList();

            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuDto> GetMenuDtoByProjectID(int id)
        {
            var tbl_MenuProjectMapping = db.MenuProjectMappings;
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
            var MenuProjectMappingQuery = from row in db.MenuProjectMappings
                                          where row.ProjectIDSys == id
                                          orderby row.MenuIDSysParent , row.Sort
                                          select row;
            
            var tbl_MenuProjectMapping = db.MenuProjectMappings;
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto = MenuProjectMappingQuery.Select(b =>
            new MenuProjectMappingDto()
            {
                MenuIDSys = b.MenuIDSys,
                ProjectIDSys = b.ProjectIDSys,
                MenuName = b.MenuName,
                MenuIDSysParent = b.MenuIDSysParent,
                Url = b.Menu_MT.Url
            }
                ).ToList();
            return MenuProjectMappingdto;
        }

        public IEnumerable<MenuProjectMappingDto> GetAllMenu(int projectid)
        {
            var MenuProjectMappingQuery = from row in db.MenuProjectMappings
                                          where row.ProjectIDSys == projectid
                                          orderby row.MenuIDSysParent, row.Sort
                                          select row;

            var tbl_MenuProjectMapping = db.MenuProjectMappings;
            IEnumerable<MenuProjectMappingDto> MenuProjectMappingdto = MenuProjectMappingQuery.Select(b =>
            new MenuProjectMappingDto()
            {
                MenuIDSys = b.MenuIDSys,
                ProjectIDSys = b.ProjectIDSys,
                MenuName = b.MenuName,
                MenuIDSysParent = b.MenuIDSysParent,
                Url = "",
                Sort = b.Sort
            }
                ).ToList();
            return MenuProjectMappingdto;

        }

        public IEnumerable<MenuProjectMappingDto> GetMenuPermission(string userid,int projectid)
        {
            var menu = from ur in db.UserRoles
                       join rp in db.RolePermission on ur.RoleID equals rp.RoleID
                       join ps in db.Permissions on rp.PermissionID equals ps.PermissionID
                       join r in db.Roles on ur.RoleID equals r.RoleID
                       join mp in db.MenuProjectMappings on ps.MenuIDSys equals mp.MenuIDSys
                       where r.ProjectIDSys == projectid && ur.UserID == userid && mp.ProjectIDSys == projectid
                       select mp;

            var tbl_MenuProjectMapping = db.MenuProjectMappings;
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
                ).ToList();
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
