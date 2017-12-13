using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using WIM.Core.Context;
using WIM.Core.Entity.Status;
using WIM.Core.Common.ValueObject;
using WIM.Core.Common;
using WIM.Core.Service.StatusManagement;
using WIM.Core.Repository.Impl.StatusManagement;
using WIM.Core.Repository.StatusManagement;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Helpers;

namespace WIM.Core.Service.Impl.StatusManagement
{
    public class StatusService : Service, IStatusService
    {

        public IEnumerable<StatusSubModuleDto> GetStatus()
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                try
                {
                    IStatusRepository repoGetLeave = new StatusRepository(db);
                    return repoGetLeave.GetDto();
                }
                catch (DbEntityValidationException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                catch (DbUpdateException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
            }

        }

        public StatusDto GetStatusByID(int id)
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                try
                {
                    IStatusRepository stByID = new StatusRepository(db);
                    return stByID.GetDto(id);
                }
                catch (DbEntityValidationException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                catch (DbUpdateException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
            }
        }

        
        public Status_MT CreateStatus(Status_MT status)
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        IStatusRepository headRepo = new StatusRepository(db);


                        Status_MT st = headRepo.Insert(status);

                        db.SaveChanges();
                        scope.Complete();
                        return st;
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                }
            }
        }

        public Status_MT CreateStatus(Status_MT status , IEnumerable<SubModuleDto> submodule)
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        List<SubModuleDto> list = submodule.ToList();
                        IStatusRepository headRepo = new StatusRepository(db);

                        Status_MT st = headRepo.Insert(status);
                        db.SaveChanges();

                        if(submodule != null)
                        {
                            int count = submodule.Count();
                            for(int i = 0; i < count ; i++)
                            {
                                StatusSubModules statusSM = new StatusSubModules();
                                statusSM.StatusIDSys = st.StatusIDSys;
                                statusSM.SubModuleIDSys = list[i].SubModuleIDSys;
                                db.StatusSubModule.Add(statusSM);
                            }
                            db.SaveChanges();
                        }
                        scope.Complete();
                        return st;
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                }
            }
        }

        public Status_MT UpdateStatus(StatusDto status)
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        Status_MT statusUpdated = new Status_MT();
                        statusUpdated.StatusIDSys = status.StatusIDSys;
                        statusUpdated.Title = status.Title;
                        IStatusRepository repo = new StatusRepository(db);
                        statusUpdated = repo.Update(statusUpdated);
                        db.SaveChanges();
                        
                        IStatusSubModuleRepository updateSM = new StatusSubModuleRepository(db);
                        List<StatusSubModules> delete = updateSM.GetMany(a => a.StatusIDSys == status.StatusIDSys).ToList();
                        int count = delete.Count();
                        for (int i = 0; i < count; i++)
                        {
                            updateSM.Delete(delete[i]);
                        }
                            db.SaveChanges();
               
                        foreach (var i in status.StatusSubModule)
                        {
                            StatusSubModules x = new CommonService().AutoMapper<StatusSubModules>(i);
                            x.StatusIDSys = status.StatusIDSys;
                            updateSM.Insert(x);
                        }
                        if (status.StatusSubModule != null)
                            db.SaveChanges();
                        
                        scope.Complete();
                        return statusUpdated;
                    }
                    catch (DbEntityValidationException)
                    {
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        scope.Dispose();
                        ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                        throw ex;
                    }
                }
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
