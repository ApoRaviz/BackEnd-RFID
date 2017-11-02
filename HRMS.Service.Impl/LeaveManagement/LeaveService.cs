using HRMS.Service.LeaveManagement;
using System;
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
using System.Data.Entity;

namespace HRMS.Service.Impl.LeaveManagement
{
    public class LeaveService : ILeaveService
    {
        private IIdentity Identity;

        public LeaveService(IIdentity identity)
        {
            Identity = identity;
        }

        public bool ApproveLeave(int id)
        {
            throw new NotImplementedException();
        }

        public bool RejectLeave(int id)
        {
            throw new NotImplementedException();
        }

        public Leave CreateLeave(Leave leave)
        {
            using (HRMSDbContext db = new HRMSDbContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        ILeaveRepository headRepo = new LeaveRepository(db, Identity);
                        ILeaveDetailRepository dRepo = new LeaveDetailRepository(db, Identity);

                        Leave leaveReq = headRepo.Insert(leave);

                        foreach (var entity in leave.LeaveDetails)
                        {
                            entity.LeaveIDSys = leave.LeaveIDSys;
                            dRepo.Insert(entity);
                        }
                        db.SaveChanges();
                        scope.Complete();
                        return leaveReq;
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

        public LeaveDto GetLeaveByID(int id)
        {
            using (HRMSDbContext db = new HRMSDbContext())
            {
                try
                {
                    ILeaveRepository repoGetLeave = new LeaveRepository(db, Identity);
                    return repoGetLeave.GetDto(id);
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

        public IEnumerable<Leave> GetLeaves()
        {
            using (HRMSDbContext db = new HRMSDbContext())
            {
                try
                {
                    ILeaveRepository repoGetLeave = new LeaveRepository(db, Identity);
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

        public IEnumerable<LeaveType> GetLeaveType()
        {
            using (HRMSDbContext db = new HRMSDbContext())
            {
                try
                {
                    ILeaveTypeRepository repoGetLT = new LeaveTypeRepository(db, Identity);
                    return repoGetLT.Get();
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

        public Leave UpdateLeave(LeaveDto leave)
        {
            using (HRMSDbContext db = new HRMSDbContext())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        Leave leaveUpdated;
                        ILeaveRepository repo = new LeaveRepository(db, Identity);
                        ILeaveDetailRepository dRepo = new LeaveDetailRepository(db, Identity);

                        leaveUpdated = repo.Update(leave);

                        dRepo.Delete(x => x.LeaveIDSys == leaveUpdated.LeaveIDSys);

                        foreach (var entity in leave.LeaveDetails)
                        {
                            var leaveForInsert = Mapper.Map<LeaveDetailDto, LeaveDetail>(entity);
                            dRepo.Insert(leaveForInsert);
                        }
                        db.SaveChanges();
                        scope.Complete();
                        return leaveUpdated;
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
