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
using WMS.Common;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Context;
using WIM.Core.Repository.Impl;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class ApiMTService : IApiMTService
    {

        ApiMTRepository repo;

        public ApiMTService()
        {
            repo = new ApiMTRepository();
        }

        public IEnumerable<Api_MT> GetAPIs()
        {
            return repo.Get(); ;
        }

        public ApiMTDto GetApiMT(string id)
        {
            Api_MT ApiMT = repo.GetByID(id);
            ApiMTDto ApiMTDto = Mapper.Map<Api_MT, ApiMTDto>(ApiMT); 
            return ApiMTDto;
        }        

        public string CreateApiMT(List<Api_MT> ApiMT)
        {
            using (var scope = new TransactionScope())
            {
                const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
                Random rnd = new Random();
                try
                {
                    for (int i = 0; i < ApiMT.Count; i++)
                {
                    var chars = Enumerable.Range(0, 4)
                    .Select(x => pool[rnd.Next(0, pool.Length)]);
                    ApiMT[i].ApiIDSys = new string(chars.ToArray());
                    repo.Insert(ApiMT[i]);
                }
                    scope.Complete();
                }
                catch (DbEntityValidationException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4011));
                    throw ex;
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

        public bool UpdateApiMT(string id, Api_MT ApiMT)
        {
            using (var scope = new TransactionScope())
            {   try
                {
                    var existedApiMT = repo.GetByID(id);
                    existedApiMT.ApiMenuMappings = ApiMT.ApiMenuMappings;
                    repo.Update(existedApiMT);
                    scope.Complete();
                }
                catch (DbEntityValidationException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
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

        public bool DeleteApiMT(string id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                     var existedApiMT = repo.GetByID(id);
                     repo.Delete(existedApiMT);
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
