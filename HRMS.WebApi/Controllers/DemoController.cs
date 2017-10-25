using HRMS.Repository.Context;
using HRMS.Repository.Entity.LeaveRequest;
using HRMS.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Status;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace HRMS.WebApi.Controllers
{

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
        public HttpResponseMessage Demo([FromBody]Leave leave)
        {
            ResponseData<int> response = new ResponseData<int>();
            try
            {
                using (HRMSDbContext db = new HRMSDbContext())
                {
                    ILeaveRepository repo = new LeaveRepository(db);
                    repo.Update(leave, "13007");

                    //ILeaveDetailRepository repoDetail = new LeaveDetailRepository(db);
                    //repoDetail.Update(new LeaveDetail(), "13007");

                    db.SaveChanges();
                }                            

                response.SetData(1);
            }
            catch (ValidationException ex)
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

    public class LeaveRepository : Repository<Leave>,  ILeaveRepository
    {
        public LeaveRepository(DbContext context)  : base(context)
        {

        }

        public IEnumerable<Leave> GetCoursesWithAuthors(int pageIndex, int pageSize)
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
