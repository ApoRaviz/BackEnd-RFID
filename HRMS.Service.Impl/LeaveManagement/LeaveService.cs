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
using WIM.Core.Common.Helpers;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using HRMS.Context;
using HRMS.Repository.LeaveManagement;
using HRMS.Repository.Impl.LeaveManagement;
using AutoMapper;
using WIM.Core.Service;
using WIM.Core.Service.Impl;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace HRMS.Service.Impl.LeaveManagement
{
    public class LeaveService : WIM.Core.Service.Impl.Service, ILeaveService
    {

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
                        ILeaveRepository headRepo = new LeaveRepository(db);
                        ILeaveDetailRepository dRepo = new LeaveDetailRepository(db);

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
                        AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    ILeaveRepository repoGetLeave = new LeaveRepository(db);
                    return repoGetLeave.GetDto(id);
                }
                catch (DbEntityValidationException)
                {
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                catch (DbUpdateException)
                {
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    ILeaveRepository repoGetLeave = new LeaveRepository(db);
                    return repoGetLeave.Get();
                }
                catch (DbEntityValidationException)
                {
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                catch (DbUpdateException)
                {
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
            }

        }

        public IEnumerable<LeaveTypeDto> GetLeaveType()
        {
            using (HRMSDbContext db = new HRMSDbContext())
            {
                try
                {
                    ILeaveTypeRepository repoGetLT = new LeaveTypeRepository(db);
                    return repoGetLT.GetDto();
                }
                catch (DbEntityValidationException)
                {
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                catch (DbUpdateException)
                {
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                        ILeaveRepository repo = new LeaveRepository(db);
                        ILeaveDetailRepository dRepo = new LeaveDetailRepository(db);
                     
                        Leave leaveForUpdate = Mapper.Map<Leave>(leave); 
                        leaveUpdated = repo.Update(leaveForUpdate);

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
                        AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                        throw ex;
                    }
                    catch (DbUpdateException)
                    {
                        scope.Dispose();
                        AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                        throw ex;
                    }
                }
            }
        }

    }
}
