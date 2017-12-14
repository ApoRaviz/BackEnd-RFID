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

using Validation = WIM.Core.Common.Utility.Validation;
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
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using WIM.Core.Entity.ProjectManagement;
using System.IO;
using System.IO.Compression;
using System.Text;
using WIM.Core.Common.Utility.Helpers;
using WIM.Core.Entity.ProjectManagement.ProjectConfigs;
using Newtonsoft.Json;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Validation;
using System.Web.Http.ModelBinding;
using WIM.Core.Entity.ProjectManagement.ProjectConfigs.Main;

namespace HRMS.WebApi.Controllers
{
    //[ValidateModel]
    [RoutePrefix("api/v1/demo")]
    public class DemoController : ApiController
    {

        private ILeaveService LeaveService;
        public DemoController(ILeaveService leaveService)
        {
            LeaveService = leaveService;
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("new")]
        public HttpResponseMessage DemoAdd([FromBody]Leave leaveRequest)
        {
            ResponseData<Leave> response = new ResponseData<Leave>();
            try
            {

                using (HRMSDbContext db = new HRMSDbContext())
                {

                    ILeaveRepository headRepo = new LeaveRepository(db);
                    ILeaveDetailRepository dRepo = new LeaveDetailRepository(db);

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

        [HttpGet]
        [Route("")]
        public HttpResponseMessage DemoUpdateGet(string data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, data.Length + 5);
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage DemoUpdateGet2()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "555 OK");
        }




        [HttpPost]
        [Route("")]
        public HttpResponseMessage DemoUpdate(string data, [FromBody]ProjectConfig projectConfig)
        {           

            Project_MT project = new Project_MT();
            ResponseData<Project_MT> response = new ResponseData<Project_MT>();

           
            using (CoreDbContext db = new CoreDbContext())
            {

                Project_MT project1 = db.Project_MT.SingleOrDefault(p => p.ProjectIDSys == 1);
                project1.ProjectConfig = projectConfig;
                db.SaveChanges();

                var projects = (
                      from p in db.Project_MT
                      select p
                ).ToList();

                try
                {
                    project = projects.FirstOrDefault(p => p.ProjectConfig.Main.Service.IsImport);
                }
                catch (NullReferenceException)
                {

                }
                response.SetData(project);

            }
            return Request.ReturnHttpResponseMessage(response);

            /*ResponseData<Leave> response = new ResponseData<Leave>();
            try
            {
                Leave leaveUpdated;
                using (HRMSDbContext db = new HRMSDbContext())
                {
                    ILeaveRepository repo = new LeaveRepository(db);
                    ILeaveDetailRepository dRepo = new LeaveDetailRepository(db);

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
            return Request.ReturnHttpResponseMessage(response);*/
        }

        [CheckModelForNull]
        [HttpPost]
        [Route("oil")]
        public HttpResponseMessage DemoUpdateOil([FromBody]Main projectConfig)
        {
            Project_MT project = new Project_MT();
            ResponseData<Project_MT> response = new ResponseData<Project_MT>();

            using (CoreDbContext db = new CoreDbContext())
            {

                Project_MT project1 = db.Project_MT.SingleOrDefault(p => p.ProjectIDSys == 1);
                project1.ProjectConfig.Main = projectConfig;
                db.SaveChanges();

                var projects = (
                      from p in db.Project_MT
                      select p
                ).ToList();

                try
                {
                    project = projects.FirstOrDefault(p => p.ProjectConfig.Main.Service.IsImport);
                }
                catch (NullReferenceException)
                {

                }
                response.SetData(project);

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

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
