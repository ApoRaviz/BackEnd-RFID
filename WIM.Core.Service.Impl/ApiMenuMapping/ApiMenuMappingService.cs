using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Common.ValueObject;
using System.Security.Principal;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common;
using WIM.Core.Entity.RoleAndPermission;

namespace WIM.Core.Service.Impl
{
    public class ApiMenuMappingService : Service, IApiMenuMappingService
    {


        public ApiMenuMappingService()
        {

        }

        public IEnumerable<ApiMenuMappingDto> GetApiMenuMapping()
        {
            IEnumerable<ApiMenuMapping> ApiMenuMappings;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IApiMenuMappingRepository repo = new ApiMenuMappingRepository(Db);
                ApiMenuMappings = repo.Get();
            }
            IEnumerable<ApiMenuMappingDto> ApiMenuMappingDtos = Mapper.Map<IEnumerable<ApiMenuMapping>, IEnumerable<ApiMenuMappingDto>>(ApiMenuMappings);
            return ApiMenuMappingDtos;
        }

        public ApiMenuMappingDto GetApiMenuMapping(string id)
        {
            ApiMenuMapping ApiMenuMapping;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IApiMenuMappingRepository repo = new ApiMenuMappingRepository(Db);
                ApiMenuMapping = repo.GetByID(id);
            }
            ApiMenuMappingDto ApiMenuMappingDto = Mapper.Map<ApiMenuMapping, ApiMenuMappingDto>(ApiMenuMapping);
            return ApiMenuMappingDto;
        }

        public IEnumerable<ApiMenuMapping> GetListApiMenuMapping(int id)
        {
            IEnumerable<ApiMenuMapping> ApiMenuMapping;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IApiMenuMappingRepository repo = new ApiMenuMappingRepository(Db);
                string[] include = { "Api_MT" };
                ApiMenuMapping = repo.GetWithInclude((c => c.MenuIDSys == id), include).ToList();
            }

            return ApiMenuMapping;
        }

        public string CreateApiMenuMapping(ApiMenuMappingDto ApiMenuMapping)
        {
            using (var scope = new TransactionScope())
            {
                ApiMenuMapping api = new ApiMenuMapping();
                ApiMenuMapping apinew = new ApiMenuMapping();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IApiMenuMappingRepository repo = new ApiMenuMappingRepository(Db);
                        api.ApiIDSys = ApiMenuMapping.ApiIDSys;
                        api.MenuIDSys = ApiMenuMapping.MenuIDSys;
                        api.GET = ApiMenuMapping.GET;
                        api.POST = ApiMenuMapping.POST;
                        api.PUT = ApiMenuMapping.PUT;
                        api.DEL = ApiMenuMapping.DEL;
                        api.Type = ApiMenuMapping.Type;
                        repo.Insert(api);
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
                return apinew.ApiIDSys;
            }
        }

        public string CreateApiMenuMapping(List<ApiMenuMappingDto> ApiMenuMapping)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IApiMenuMappingRepository repo = new ApiMenuMappingRepository(Db);
                        foreach (var c in ApiMenuMapping)
                        {
                            ApiMenuMapping api = new ApiMenuMapping();
                            api.ApiIDSys = c.ApiIDSys;
                            api.MenuIDSys = c.MenuIDSys;
                            api.GET = c.GET;
                            api.POST = c.POST;
                            api.PUT = c.PUT;
                            api.DEL = c.DEL;
                            api.Type = c.Type;
                            repo.Insert(api);
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
                return "Success";
            }
        }

        public bool UpdateApiInMenu(List<ApiMenuMappingDto> ApiMenuMapping)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        CoreDbContext db = new CoreDbContext();
                        IApiMenuMappingRepository repo = new ApiMenuMappingRepository(Db);
                        IPermissionRepository repoPermission = new PermissionRepository(Db);
                        IRepository<RolePermissions> repoRole = new Repository<RolePermissions>(Db);
                        var ispermission = repoPermission.GetPermissionHasCreated(ApiMenuMapping[0].MenuIDSys);
                        string[] include = { "Role" };
                        var isrole = repoRole.GetWithInclude(a => (ispermission.Select(c => c.PermissionID).Contains(a.PermissionID)), include)
                            .Select(m => new Role()
                            {
                                RoleID = m.Role.RoleID,
                                ProjectIDSys = m.Role.ProjectIDSys
                            }
                                   ).ToList();
                        List<ApiMenuMapping> newapi = new List<ApiMenuMapping>();
                        foreach (var c in ApiMenuMapping)
                        {
                            ApiMenuMapping api = new ApiMenuMapping();
                            api = new CommonService().AutoMapper<ApiMenuMapping>(c);
                            newapi.Add(repo.Insert(api));
                        }
                        Db.SaveChanges();
                        if (ispermission != null)
                        {
                            List<Permission> permission = new List<Permission>();
                            foreach (var c in newapi)
                            {

                                if (c.Type == "A")
                                {
                                    foreach (var i in ispermission)
                                    {
                                        Permission data = new Permission();
                                        data.ApiIDSys = c.ApiIDSys;
                                        data.MenuIDSys = c.MenuIDSys;
                                        data.ProjectIDSys = i.ProjectIDSys;
                                        if (c.GET)
                                        {
                                            data.PermissionID = Guid.NewGuid().ToString();
                                            data.PermissionName = "GET " + i.PermissionName;
                                            data.Method = "GET";
                                            permission.Add(repoPermission.Insert(data));
                                        }
                                        if (c.POST)
                                        {
                                            data.PermissionID = Guid.NewGuid().ToString();
                                            data.PermissionName = "POST " + i.PermissionName;
                                            data.Method = "POST";
                                            permission.Add(repoPermission.Insert(data));
                                        }
                                        if (c.PUT)
                                        {
                                            data.PermissionID = Guid.NewGuid().ToString();
                                            data.PermissionName = "PUT " + i.PermissionName;
                                            data.Method = "PUT";
                                            permission.Add(repoPermission.Insert(data));
                                        }
                                        if (c.DEL)
                                        {
                                            data.PermissionID = Guid.NewGuid().ToString();
                                            data.PermissionName = "DELETE " + i.PermissionName;
                                            data.Method = "DELETE";
                                            permission.Add(repoPermission.Insert(data));
                                        }
                                    }
                                    Db.SaveChanges();
                                    if (isrole != null)
                                    {
                                        var role = isrole.GroupBy(a => a.ProjectIDSys).Select(grp => grp.ToList()).ToList();
                                        var permissions = permission.GroupBy(a => a.ProjectIDSys).Select(grp => grp.ToList()).ToList();
                                        foreach(var permiss in permissions)
                                        {
                                            foreach(var ro in role)
                                            {
                                                if(ro[0].ProjectIDSys == permiss[0].ProjectIDSys)
                                                {
                                                    foreach(var i in permiss)
                                                    {
                                                        foreach(var r in ro)
                                                        {
                                                            RolePermissions a = new RolePermissions();
                                                            a.PermissionID = i.PermissionID;
                                                            a.RoleID = r.RoleID;
                                                            repoRole.Insert(a);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        Db.SaveChanges();
                                    }
                                }
                            }
                        }
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

        public bool UpdateApiMenuMapping(ApiMenuMapping ApiMenuMapping)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IApiMenuMappingRepository repo = new ApiMenuMappingRepository(Db);
                        repo.Update(ApiMenuMapping);
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

        public bool DeleteApiMenuMapping(string id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IApiMenuMappingRepository repo = new ApiMenuMappingRepository(Db);
                        var existedApiMenuMapping = repo.GetByID(id);
                        repo.Delete(existedApiMenuMapping);
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

    }
}
