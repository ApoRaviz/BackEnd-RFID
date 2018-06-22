using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WIM.Core.Context;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Security.Principal;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Service;
using WIM.Core.Entity.Status;
using WIM.Core.Repository.StatusManagement;
using WIM.Core.Repository.Impl.StatusManagement;

namespace WIM.Core.Service.Impl
{
    public class SubModuleService : Service, ISubModuleService
    {
        public SubModuleService()
        {
        }


        public IEnumerable<SubModules> GetSubModules()
        {
            IEnumerable<SubModules> modules;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ISubModuleRepository repo = new SubModuleRepository(Db);
                modules = repo.Get();
            }
            return modules;
        }

        public SubModules GetSubModulesByID(int id)
        {
            SubModules modules;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ISubModuleRepository repo = new SubModuleRepository(Db);
                modules = repo.GetByID(id);
            }
            return modules;
        }

        public IEnumerable<SubModules> GetSubModulesByModuleID(int id)
        {
            IEnumerable<SubModules> modules;
            using (CoreDbContext Db = new CoreDbContext())
            {
                ISubModuleRepository repo = new SubModuleRepository(Db);
                modules = repo.GetMany(a => a.ModuleIDSys == id);
            }
            return modules;
        }

        public SubModules CreateModule(SubModules module)
        {
            using (var scope = new TransactionScope())
            {
                SubModules moduleNew = new SubModules();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ISubModuleRepository repo = new SubModuleRepository(Db);
                        moduleNew = repo.Insert(module);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw e;
                }
                return moduleNew;
            }
        }

        public bool UpdateModule(SubModules module)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ISubModuleRepository repo = new SubModuleRepository(Db);
                        repo.Update(module);
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

        public bool DeleteModule(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        ISubModuleRepository repo = new SubModuleRepository(Db);
                        repo.Delete(id);
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

