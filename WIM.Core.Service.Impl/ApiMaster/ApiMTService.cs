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
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Context;
using WIM.Core.Service;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Common.ValueObject;
using System.Security.Principal;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WIM.Core.Service.Impl
{
    public class ApiMTService : Service , IApiMTService
    {

        public ApiMTService()
        {

        }

        public IEnumerable<Api_MT> GetAPIs()
        {
            IEnumerable<Api_MT> apis;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IApiMTRepository repo = new ApiMTRepository(Db);
                apis = repo.Get();
            }
            return apis;
        }

        public ApiMTDto GetApiMT(string id)
        {
            Api_MT ApiMT;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IApiMTRepository repo = new ApiMTRepository(Db);
                ApiMT = repo.GetByID(id);
            }
            ApiMTDto ApiMTDto = Mapper.Map<Api_MT, ApiMTDto>(ApiMT);
            return ApiMTDto;
        }

        public IEnumerable<Api_MT> GetAPIs(int Moduleidsys)
        {
            IEnumerable<Api_MT> apis;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IApiMTRepository repo = new ApiMTRepository(Db);
                apis = repo.GetMany(a => a.ModuleIDSys == Moduleidsys);
            }
            return apis;
        }

        public string CreateApiMT(List<Api_MT> ApiMT)
        {
            using (var scope = new TransactionScope())
            {
                const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
                Random rnd = new Random();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IApiMTRepository repo = new ApiMTRepository(Db);
                        for (int i = 0; i < ApiMT.Count; i++)
                        {
                            var chars = Enumerable.Range(0, 4)
                            .Select(x => pool[rnd.Next(0, pool.Length)]);
                            ApiMT[i].ApiIDSys = new string(chars.ToArray());
                            repo.Insert(ApiMT[i]);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
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

        public bool UpdateApiMT(Api_MT ApiMT)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IApiMTRepository repo = new ApiMTRepository(Db);
                        repo.Update(ApiMT);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
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

        public bool DeleteApiMT(string id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IApiMTRepository repo = new ApiMTRepository(Db);
                        var existedApiMT = repo.GetByID(id);
                        repo.Delete(existedApiMT);
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

        public bool DeleteApiMT(List<string> arrApi)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IApiMTRepository repo = new ApiMTRepository(Db);
                        foreach (var item in arrApi)
                        {
                            //var existedApiMT = repo.GetByID(id);
                            repo.Delete(x=>x.ApiIDSys == item);
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

    }
}
