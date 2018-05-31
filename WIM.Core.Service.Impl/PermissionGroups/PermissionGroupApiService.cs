using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository.Impl.ApiMaster;
using WIM.Core.Repository.MenuAndPermission;
using WIM.Core.Service.PermissionGroups;

namespace WIM.Core.Service.Impl.PermissionGroups
{
    public class PermissionGroupApiService : Service, IPermissionGroupApiService
    {
        public IEnumerable<PermissionGroupApi> GetPermissionGroup()
        {
            IEnumerable<PermissionGroupApi> group;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionGroupApiRepository repo = new PermissionGroupApiRepository(Db);
                group = repo.Get();
            }
            return group;
        }

        public IEnumerable<PermissionGroupApi> GetGroupApiByGroupIDSys(string id)
        {
            IEnumerable<PermissionGroupApi> group;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionGroupApiRepository repo = new PermissionGroupApiRepository(Db);
                group = repo.GetMany(a => a.GroupIDSys == id);
            }
            return group;
        }

        public bool CreateGroup(IEnumerable<PermissionGroupApi> PermissionGroup)
        {
            using (var scope = new TransactionScope())
            {
                PermissionGroup group = new PermissionGroup();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionGroupApiRepository repo = new PermissionGroupApiRepository(Db);
                        foreach(var api in PermissionGroup)
                        {
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
                    ValidationException ex = new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }

                return true;
            }
        }

        public bool UpdateGroup(IEnumerable<PermissionGroupApi> PermissionGroup)
        {
            throw new NotImplementedException();
        }

        public bool DeleteGroup(string id)
        {
            throw new NotImplementedException();
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
