
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Entity.LeaveManagement;
using HRMS.Common.ValueObject.LeaveManagement;
using System.Security.Principal;
using System.Transactions;
using WIM.Core.Common.Validation;
using WIM.Core.Common.Helpers;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using HRMS.Context;
using HRMS.Repository.LeaveManagement;
using HRMS.Repository.Impl.LeaveManagement;
using AutoMapper;
using WIM.Core.Service;
using WIM.Core.Service.Impl;
using HRMS.Service.StatusManagement;
using WIM.Core.Context;
using WIM.Core.Entity.Status;
using HRMS.Repository.Impl.StatusManagement;
using HRMS.Repository.StatusManagement;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Common.ValueObject;

namespace HRMS.Service.Impl.StatusManagement
{
    public class StatusService : WIM.Core.Service.Impl.Service, IStatusService
    {

        public IEnumerable<Status_MT> GetStatus()
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                try
                {
                    IStatusRepository repoGetLeave = new StatusRepository(db);
                    return repoGetLeave.Get();
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

        public Status_MT UpdateStatus(Status_MT status)
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        Status_MT statusUpdated;
                        IStatusRepository repo = new StatusRepository(db);
                        statusUpdated = repo.Update(status);
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

        public IEnumerable<ProjectDto> GetProject()
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                try
                {
                    IProjectRepository project = new ProjectRepository(db);
                    return project.GetDto();
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
