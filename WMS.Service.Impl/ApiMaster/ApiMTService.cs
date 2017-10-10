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
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;

namespace WMS.Service
{
    public class ApiMTService : IApiMTService
    {
        private MasterContext db = MasterContext.Create();
        private GenericRepository<Api_MT> repo;

        public ApiMTService()
        {
            repo = new GenericRepository<Api_MT>(db);
        }

        public IEnumerable<Api_MT> GetAPIs()
        {
            
            return repo.GetAll(); ;
        }

        public ApiMTDto GetApiMT(string id)
        {
            Api_MT ApiMT = (from i in db.Api_MT
                            where i.ApiIDSys == id 
                            select i).SingleOrDefault();

            ApiMTDto ApiMTDto = Mapper.Map<Api_MT, ApiMTDto>(ApiMT); 

            return ApiMTDto;
        }        

        public string CreateApiMT(List<Api_MT> ApiMT)
        {
            using (var scope = new TransactionScope())
            {
                const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
                Random rnd = new Random();
                for (int i = 0; i < ApiMT.Count; i++)
                {
                    var chars = Enumerable.Range(0, 4)
                    .Select(x => pool[rnd.Next(0, pool.Length)]);
                    ApiMT[i].ApiIDSys = new string(chars.ToArray());
                    repo.Insert(ApiMT[i]);
                }
                try
                {
                    db.SaveChanges();
                    scope.Complete();
                }
                catch (DbEntityValidationException)
                {
                    db.Dispose();
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4011));
                    throw ex;
                }
                catch (DbUpdateException)
                {
                    db.Dispose();
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
            {
                var existedApiMT = repo.GetByID(id);
                existedApiMT.ApiMenuMappings = ApiMT.ApiMenuMappings;
                repo.Update(existedApiMT);
                try
                {
                    db.SaveChanges();
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
                scope.Complete();
                return true;
            }
        }

        public bool DeleteApiMT(string id)
        {
            using (var scope = new TransactionScope())
            {
                var existedApiMT = repo.GetByID(id);
                repo.Delete(existedApiMT);

                try
                {
                    db.SaveChanges();
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
