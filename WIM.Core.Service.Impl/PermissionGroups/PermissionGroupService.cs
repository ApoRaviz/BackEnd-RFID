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
using WIM.Core.Repository.ApiMaster;
using WIM.Core.Repository.Impl.ApiMaster;
using WIM.Core.Repository.MenuAndPermission;
using WIM.Core.Service.PermissionGroups;

namespace WIM.Core.Service.Impl.PermissionGroups
{
    public class PermissionGroupService : Service, IPermissionGroupService
    {
        public PermissionGroup GetGroupByGroupIDSys(string id)
        {
            PermissionGroup group;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionGroupRepository repo = new PermissionGroupRepository(Db);
                group = repo.GetByID(id);
            }
            return group;
        }

        public IEnumerable<PermissionGroup> GetGroupByMenuIDSys(int id)
        {
            IEnumerable<PermissionGroup> group;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionGroupRepository repo = new PermissionGroupRepository(Db);
                string[] include = { "PermissionGroupApi" };
                group = repo.GetPermissionGroupWithInclude(id);
            }
            return group;
        }

        public IEnumerable<PermissionGroup> GetPermissionGroup()
        {
            IEnumerable<PermissionGroup> group;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionGroupRepository repo = new PermissionGroupRepository(Db);
                group = repo.Get();
            }
            return group;
        }

        //public string CreateGroup(PermissionGroup PermissionGroup)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        PermissionGroup group = new PermissionGroup();
        //        try
        //        {
        //            using (CoreDbContext Db = new CoreDbContext())
        //            {
        //                IPermissionGroupRepository repo = new PermissionGroupRepository(Db);
        //                PermissionGroup.GroupIDSys = Guid.NewGuid().ToString();
        //                group = repo.Insert(PermissionGroup);
        //                Db.SaveChanges();
        //                scope.Complete();
        //            }
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
        //            throw ex;
        //        }

        //        return group.GroupIDSys;
        //    }
        //}

        public bool CreateGroup(IEnumerable<PermissionGroup> PermissionGroup)
        {
            using (var scope = new TransactionScope())
            {
                
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionGroupRepository repo = new PermissionGroupRepository(Db);
                        IPermissionGroupApiRepository repo2 = new PermissionGroupApiRepository(Db);
                        List<PermissionGroup> forID = new List<PermissionGroup>();
                        foreach (var group in PermissionGroup)
                        {
                            group.GroupIDSys = Guid.NewGuid().ToString();
                            forID.Add(repo.Insert(group));
                        }
                        Db.SaveChanges();
                        List<PermissionGroupApi> forApi = new List<PermissionGroupApi>();
                        foreach(var group in PermissionGroup)
                        {
                            foreach(var api in group.PermissionGroupApi)
                            {
                                api.GroupIDSys = group.GroupIDSys;
                                forApi.Add(repo2.Insert(api));
                            }
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
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }

                return true;
            }
        }

        public bool CreateApi(IEnumerable<PermissionGroupApi> PermissionGroup)
        {
            using (var scope = new TransactionScope())
            {

                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionGroupApiRepository repo2 = new PermissionGroupApiRepository(Db);
                        
                        List<PermissionGroupApi> forApi = new List<PermissionGroupApi>();
                        foreach (var group in PermissionGroup)
                        {
                            forApi.Add(repo2.Insert(group));
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
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }

                return true;
            }
        }

        public bool UpdateGroup(PermissionGroup PermissionGroup)
        {
            using (var scope = new TransactionScope())
            {
                PermissionGroup group = new PermissionGroup();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IPermissionGroupRepository repo = new PermissionGroupRepository(Db);
                        group = repo.Update(PermissionGroup);
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
