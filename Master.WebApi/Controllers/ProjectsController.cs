using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Service;
using WIM.Core.Service.Impl;

namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/projects")]
    public class ProjectsController : ApiController
    {
        private IProjectService ProjectService;
        public ProjectsController(IProjectService projectService)
        {
            this.ProjectService = projectService;
        }

        // GET: api/projects
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Project_MT>> response = new ResponseData<IEnumerable<Project_MT>>();
            try
            {
                IEnumerable<Project_MT> Projects;
                if (User.IsSysAdmin())
                {
                    Projects = ProjectService.GetProjects();
                }
                else
                {
                    Projects = ProjectService.GetProjects(User.Identity.GetProjectIDSys());
                }
                
                response.SetData(Projects);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //// GET: api/projects
        //[HttpGet]
        //[Route("/customer/{}")]
        //public HttpResponseMessage GetProjectsByCusID()
        //{
        //    ResponseData<object> response = new ResponseData<object>();
        //    try
        //    {
        //        object Projects = ProjectService.GetProjectsByCusID();
        //        response.SetData(Projects);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        response.SetErrors(ex.Errors);
        //        response.SetStatus(HttpStatusCode.PreconditionFailed);
        //    }
        //    return Request.ReturnHttpResponseMessage(response);
        //}



        // GET: api/projects
        [HttpGet]
        [Route("menu/{CusIDSys}")]
        public HttpResponseMessage GetProjectMenu(int CusIDSys)
        {
            ResponseData<List<Project_MT>> response = new ResponseData<List<Project_MT>>();
            try
            {
                List<Project_MT> Projects = ProjectService.ProjectHaveMenu(CusIDSys);
                response.SetData(Projects);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/projects
        [HttpGet]
        [Route("customer/{CusIDSys}")]
        public HttpResponseMessage GetCustomerProject(int CusIDSys)
        {
            ResponseData<List<Project_MT>> response = new ResponseData<List<Project_MT>>();
            try
            {
                List<Project_MT> Projects = ProjectService.ProjectCustomer(CusIDSys);
                response.SetData(Projects);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("select")]
        public HttpResponseMessage GetSelect()
        {
            
                IResponseData<IEnumerable<Project_MT>> response = new ResponseData<IEnumerable<Project_MT>>();
            IEnumerable<Project_MT> Project;
                try
                {
                if (User.IsSysAdmin())
                {
                    Project = ProjectService.GetProjects();
                }
                else
                {
                    Project = ProjectService.GetProjects().Where(a => a.ProjectIDSys == User.Identity.GetProjectIDSys());
                }
                    response.SetData(Project);
                }
                catch (ValidationException ex)
                {
                    response.SetErrors(ex.Errors);
                    response.SetStatus(HttpStatusCode.PreconditionFailed);
                }
                return Request.ReturnHttpResponseMessage(response);
            
        }


        // GET: api/Projects/5
        [HttpGet]
        [Route("{projectIDSys}")]
        public HttpResponseMessage Get(int projectIDSys, [FromUri]List<string> include)
        {
            if (include.Any(x => x.Equals("customer_mt", StringComparison.CurrentCultureIgnoreCase)))
            {
                IResponseData<Project_MT> response = new ResponseData<Project_MT>();
                try
                {
                    Project_MT Project = ProjectService.GetProjectByProjectIDSysIncludeCustomer(projectIDSys);
                    response.SetData(Project);
                }
                catch (ValidationException ex)
                {
                    response.SetErrors(ex.Errors);
                    response.SetStatus(HttpStatusCode.PreconditionFailed);
                }
                return Request.ReturnHttpResponseMessage(response);
            }
            else
            {
                IResponseData<Project_MT> response = new ResponseData<Project_MT>();
                try
                {
                    Project_MT Project = ProjectService.GetProjectByProjectIDSys(projectIDSys);
                    response.SetData(Project);
                }
                catch (ValidationException ex)
                {
                    response.SetErrors(ex.Errors);
                    response.SetStatus(HttpStatusCode.PreconditionFailed);
                }
                return Request.ReturnHttpResponseMessage(response);
            }
        }

        // POST: api/Projects
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Project_MT project)
        {
            IResponseData<Project_MT> response = new ResponseData<Project_MT>();
            try
            {
                project.CusIDSys = project.Customer_MT.CusIDSys;
                project.Customer_MT = null;
                Project_MT newProject = ProjectService.CreateProject(project);
                response.SetData(newProject);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //[HttpPost]
        //[Route("user/{UserID}")]
        //public HttpResponseMessage PostUserProject(string UserID, [FromBody]List<Project_MT> project)
        //{
        //    IResponseData<bool> response = new ResponseData<bool>();
        //    try
        //    {
        //        bool newProject = false;
        //        for (int i = 0; i < project.Count; i++)
        //        {
        //            newProject = ProjectService.CreateUserProject(UserID, project[i].ProjectIDSys);
        //        }

        //        response.SetData(newProject);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        response.SetErrors(ex.Errors);
        //        response.SetStatus(HttpStatusCode.PreconditionFailed);
        //    }
        //    return Request.ReturnHttpResponseMessage(response);
        //}
        // PUT: api/Projects/5
        [HttpPut]
        [Route("{projectIDSys}")]
        public HttpResponseMessage Put(int projectIDSys, [FromBody]Project_MT project)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                project.Customer_MT = null;
                bool isUpated = ProjectService.UpdateProject(project);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Projects/5
        [HttpDelete]
        [Route("{projectIDSys}")]
        public HttpResponseMessage Delete(int projectIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ProjectService.DeleteProject(projectIDSys);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Projects/5
        //    [HttpDelete]
        //    [Route("user/{projectIDSys}")]
        //    public HttpResponseMessage DeleteUserProject(int projectIDSys)
        //    {
        //        IResponseData<bool> response = new ResponseData<bool>();
        //        try
        //        {
        //            bool isUpated = ProjectService.DeleteUserProject(projectIDSys, User.Identity.GetUserId());
        //            response.SetData(isUpated);
        //        }
        //        catch (ValidationException ex)
        //        {
        //            response.SetErrors(ex.Errors);
        //            response.SetStatus(HttpStatusCode.PreconditionFailed);
        //        }
        //        return Request.ReturnHttpResponseMessage(response);
        //    }
        //}

        public class HttpRequestParameter
        {
            public List<string> Includes { get; set; }
        }
    }
}