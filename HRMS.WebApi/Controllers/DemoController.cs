using AutoMapper;
using HRMS.Repository.Context;
using HRMS.Repository.Entity.LeaveRequest;
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

namespace HRMS.WebApi.Controllers
{   

    public class LeaveDto
    {
        [Key]
        public int LeaveIDSys { get; set; }
        public int StatusIDSys { get; set; }
        public string StatusTitle { get; set; }
        public Decimal Duration { get; set; }
        public string Comment { get; set; }
        public int LeaveTypeIDSys { get; set; }
        public string EmID { get; set; }
    }


    [RoutePrefix("api/v1/demo")]
    public class DemoController : ApiController
    {
        private IDemoService DemoService;

        public DemoController(IDemoService demoService)
        {
            DemoService = demoService;
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Demo([FromBody]LeaveDto leaveRequest)
        {
            ResponseData<Leave> response = new ResponseData<Leave>();
            try
            {
                Leave leaveUpdated;
                using (HRMSDbContext db = new HRMSDbContext())
                {
                    ILeaveRepository repo = new LeaveRepository(db);
                    User.Identity.GetP
                    leaveUpdated = repo.GetWithInclude(x => x.LeaveIDSys == 2, "LeaveDetails").First();

                    //leaveUpdated = repo.Update(leaveRequest, "13007");                                      
                    //db.SaveChanges();
                }
                LeaveDto leaveReturn = Mapper.Map<Leave, LeaveDto>(leaveUpdated);
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

    public interface ILeaveRepository : IRepository<Leave>
    {
        IEnumerable<Leave> GetTopSellingCourses(int count);
        IEnumerable<Leave> GetCoursesWithAuthors(int pageIndex, int pageSize);
    }

    public class LeaveRepository : Repository<Leave>, ILeaveRepository
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


    }

    public interface ILeaveDetailRepository : IRepository<LeaveDetail>
    {


    }


    public class LeaveDetailRepository : Repository<LeaveDetail>, ILeaveDetailRepository
    {
        public LeaveDetailRepository(DbContext context) : base(context)
        {

        }
    }

}
