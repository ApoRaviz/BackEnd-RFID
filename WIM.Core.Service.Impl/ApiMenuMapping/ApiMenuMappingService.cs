using AutoMapper;
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
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Common.ValueObject;
using System.Security.Principal;

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
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
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
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return "Success";
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

    }
}
