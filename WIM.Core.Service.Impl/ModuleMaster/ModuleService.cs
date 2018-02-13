using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Transactions;
using System.Data.Entity.Infrastructure;
using WIM.Core.Context;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Entity.Module;

namespace WIM.Core.Service.Impl
{
    public class ModuleService : Service, IModuleService
    {
        public ModuleService()
        {
        }


        public IEnumerable<Module_MT> GetModules()
        {
            IEnumerable<Module_MT> modules;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IModuleRepository repo = new ModuleRepository(Db);
                modules = repo.Get();
            }
            return modules;
        }

        public Module_MT GetProjectByModuleIDSys(int id)
        {
            Module_MT modules;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IModuleRepository repo = new ModuleRepository(Db);
                modules = repo.GetByID(id);
            }
            return modules;
        }

        public Module_MT CreateModule(Module_MT module)
        {
            using (var scope = new TransactionScope())
            {
                Module_MT moduleNew = new Module_MT();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IModuleRepository repo = new ModuleRepository(Db);
                        moduleNew = repo.Insert(module);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw e;
                }
                return moduleNew;
            }
        }

        public bool UpdateModule(Module_MT module)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IModuleRepository repo = new ModuleRepository(Db);
                        repo.Update(module);
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
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
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
                        IModuleRepository repo = new ModuleRepository(Db);
                        repo.Delete(id);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4017));
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

