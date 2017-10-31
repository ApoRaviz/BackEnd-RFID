using AutoMapper;
using HRMS.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using Validation = WIM.Core.Common.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Status;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using System.Linq.Expressions;
using HRMS.Entity.LeaveManagement;
using HRMS.Context;
using HRMS.Repository.Impl.LeaveManagement;
using HRMS.Repository.LeaveManagement;
using HRMS.Service.LeaveManagement;
using HRMS.Service.Impl.LeaveManagement;
using HRMS.Common.ValueObject.LeaveManagement;

namespace HRMS.WebApi.Controllers
{    
    [RoutePrefix("api/v1/demo")]
    public class DemoController : ApiController
    {

        private ILeaveService LeaveService;
        public DemoController(ILeaveService leaveService)
        {
            LeaveService = leaveService;           
        }
                
        [HttpPost]
        [Route("new")]
        public HttpResponseMessage DemoAdd([FromBody]Leave leaveRequest)
        {
            ResponseData<Leave> response = new ResponseData<Leave>();
            try
            {

                using (HRMSDbContext db = new HRMSDbContext())
                {

                    string y = LeaveService.GetName();

                    ILeaveRepository headRepo = new LeaveRepository(db, User.Identity);
                    ILeaveDetailRepository dRepo = new LeaveDetailRepository(db, User.Identity);

                    Leave x = headRepo.Insert(leaveRequest);

                    foreach (var entity in leaveRequest.LeaveDetails)
                    {
                        entity.LeaveIDSys = x.LeaveIDSys;
                        dRepo.Insert(entity);
                    }

                    db.SaveChanges();
                }
                
                response.SetData(leaveRequest);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage DemoUpdate([FromBody]LeaveDto leaveRequest)
        {
            ResponseData<Leave> response = new ResponseData<Leave>();
            try
            {
                Leave leaveUpdated;
                using (HRMSDbContext db = new HRMSDbContext())
                {
                    ILeaveRepository repo = new LeaveRepository(db, User.Identity);
                    ILeaveDetailRepository dRepo = new LeaveDetailRepository(db, User.Identity);

                    leaveUpdated = repo.Update(leaveRequest);
                 
                    dRepo.Delete(x => x.LeaveIDSys == leaveUpdated.LeaveIDSys);
          
                    foreach (var entity in leaveRequest.LeaveDetails)
                    {
                        var leaveForInsert = Mapper.Map<LeaveDetailDto, LeaveDetail> (entity);
                        dRepo.Insert(leaveForInsert);
                    }

                    db.SaveChanges();
                }

                response.SetData(leaveUpdated);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        } 
        
    }

    /*public interface ILeaveRepository : IRepository<Leave>
    {
        IEnumerable<Leave> GetTopSellingCourses(int count);
        IEnumerable<Leave> GetCoursesWithAuthors(int pageIndex, int pageSize);
    }*/

    /*public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private HRMSDbContext Db;

        public LeaveRepository(HRMSDbContext context) : base(context)
        {
            Db = context;
        }
       
        public IEnumerable<Leave> GetCoursesWithAuthors(int pageIndex, int pageSize)
        {

            throw new NotImplementedException();
        }

        public IEnumerable<LeaveDto> GetDto(int limit)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Leave> GetTopSellingCourses(int count)
        {
            throw new NotImplementedException();
        }


    }*/

    /*public interface ILeaveDetailRepository : IRepository<LeaveDetail>
    {


    }


    public class LeaveDetailRepository : Repository<LeaveDetail>, ILeaveDetailRepository
    {
        public LeaveDetailRepository(DbContext context) : base(context)
        {

        }
    }*/

}
