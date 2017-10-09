using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Master;
using WMS.Master.Unit;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;

namespace WMS.Master
{
    public class ApiMenuMappingService : IApiMenuMappingService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<ApiMenuMapping> repo;

        public ApiMenuMappingService()
        {
            repo = new GenericRepository<ApiMenuMapping>(db);
        }

        public IEnumerable<ApiMenuMappingDto> GetCategories()
        {
            IEnumerable<ApiMenuMapping> ApiMenuMappings = (from i in db.ApiMenuMappings
                                          select i).ToList();

            IEnumerable<ApiMenuMappingDto> ApiMenuMappingDtos = Mapper.Map<IEnumerable<ApiMenuMapping>, IEnumerable<ApiMenuMappingDto>>(ApiMenuMappings);
            return ApiMenuMappingDtos;
        }

        public ApiMenuMappingDto GetApiMenuMapping(string id)
        {
            ApiMenuMapping ApiMenuMapping = (from i in db.ApiMenuMappings
                            where i.ApiIDSys == id 
                            select i).SingleOrDefault();

            ApiMenuMappingDto ApiMenuMappingDto = Mapper.Map<ApiMenuMapping, ApiMenuMappingDto>(ApiMenuMapping); 

            return ApiMenuMappingDto;
        }

        public List<ApiMenuMapping> GetListApiMenuMapping(int id)
        {
            var ApiMenuMapping = (from i in db.ApiMenuMappings
                                             where i.MenuIDSys == id
                                             select i).ToList();
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
                return api.ApiIDSys;
            }
        }

        public string CreateApiMenuMapping(List<ApiMenuMappingDto> ApiMenuMapping)
        {
            using (var scope = new TransactionScope())
            {
                ApiMenuMapping api = new ApiMenuMapping();
                
                    foreach (var c in ApiMenuMapping)
                    {
                        api.ApiIDSys = c.ApiIDSys;
                        api.MenuIDSys = c.MenuIDSys;
                        api.GET = c.GET;
                        api.POST = c.POST;
                        api.PUT = c.PUT;
                        api.DEL = c.DEL;
                        api.Type = c.Type;
                        repo.Insert(api);
                    }

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
                return api.ApiIDSys;
            }
        }

        public bool UpdateApiMenuMapping(string id, ApiMenuMapping ApiMenuMapping)
        {
            using (var scope = new TransactionScope())
            {
                repo.Update(ApiMenuMapping);
                try
                {
                    db.SaveChanges();
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
                scope.Complete();
                return true;
            }
        }

        public bool DeleteApiMenuMapping(string id)
        {
            using (var scope = new TransactionScope())
            {
                var existedApiMenuMapping = repo.GetByID(id);
                repo.Delete(existedApiMenuMapping);
                try
                {
                db.SaveChanges();
                scope.Complete();
                }catch (DbUpdateConcurrencyException e)
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
