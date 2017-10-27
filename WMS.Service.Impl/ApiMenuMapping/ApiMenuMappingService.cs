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
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class ApiMenuMappingService : IApiMenuMappingService
    {
        private IApiMenuMappingRepository repo;

        public ApiMenuMappingService()
        {
            repo = new ApiMenuMappingRepository();
        }

        public IEnumerable<ApiMenuMappingDto> GetApiMenuMapping()
        {
            IEnumerable<ApiMenuMapping> ApiMenuMappings = repo.Get();

            IEnumerable<ApiMenuMappingDto> ApiMenuMappingDtos = Mapper.Map<IEnumerable<ApiMenuMapping>, IEnumerable<ApiMenuMappingDto>>(ApiMenuMappings);
            return ApiMenuMappingDtos;
        }

        public ApiMenuMappingDto GetApiMenuMapping(string id)
        {
            ApiMenuMapping ApiMenuMapping = repo.GetByID(id);

            ApiMenuMappingDto ApiMenuMappingDto = Mapper.Map<ApiMenuMapping, ApiMenuMappingDto>(ApiMenuMapping); 

            return ApiMenuMappingDto;
        }

        public IEnumerable<ApiMenuMapping> GetListApiMenuMapping(int id)
        {
            var ApiMenuMapping = repo.Get(id);
            return ApiMenuMapping;
        }

        public string CreateApiMenuMapping(ApiMenuMappingDto ApiMenuMapping)
        {
            using (var scope = new TransactionScope())
            {       
                ApiMenuMapping api = new ApiMenuMapping();
                try
                {
                    api.ApiIDSys = ApiMenuMapping.ApiIDSys;
                    api.MenuIDSys = ApiMenuMapping.MenuIDSys;
                    api.GET = ApiMenuMapping.GET;
                    api.POST = ApiMenuMapping.POST;
                    api.PUT = ApiMenuMapping.PUT;
                    api.DEL = ApiMenuMapping.DEL;
                    api.Type = ApiMenuMapping.Type;
                    repo.Insert(api);
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
                return api.ApiIDSys;
            }
        }

        public string CreateApiMenuMapping(List<ApiMenuMappingDto> ApiMenuMapping)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    foreach (var c in ApiMenuMapping)
                    {   ApiMenuMapping api = new ApiMenuMapping();
                        api.ApiIDSys = c.ApiIDSys;
                        api.MenuIDSys = c.MenuIDSys;
                        api.GET = c.GET;
                        api.POST = c.POST;
                        api.PUT = c.PUT;
                        api.DEL = c.DEL;
                        api.Type = c.Type;
                        repo.Insert(api);
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
                return "Success";
            }
        }

        public bool UpdateApiMenuMapping(string id, ApiMenuMapping ApiMenuMapping)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    repo.Update(ApiMenuMapping);
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
                return true;
            }
        }

        public bool DeleteApiMenuMapping(string id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                var existedApiMenuMapping = repo.GetByID(id);
                repo.Delete(existedApiMenuMapping);
                scope.Complete();
                }catch (DbUpdateConcurrencyException)
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
