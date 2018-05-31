using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Service;


namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Roles")]
    public class RolesController : ApiController
    {
        private IRoleService RoleService;


        public RolesController(IRoleService RoleService)
        {
            this.RoleService = RoleService;
        }

        //get api/Roles
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Role>> response = new ResponseData<IEnumerable<Role>>();
            try
            {
                IEnumerable<Role> Role;
                if (User.IsSysAdmin())
                {
                    Role = RoleService.GetRoles();
                }
                else
                {
                    int x = User.Identity.GetProjectIDSys();
                    Role = RoleService.GetRoles(User.Identity.GetProjectIDSys());
                }
                
                response.SetData(Role);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("role/{PermissionID}")]
        public HttpResponseMessage GetRoleNotInPermission(string permissionID)
        {
            ResponseData<IEnumerable<RolePermissionDto>> response = new ResponseData<IEnumerable<RolePermissionDto>>();
            try
            {
                IEnumerable<RolePermissionDto> Role = RoleService.GetRoleNotPermissionID(permissionID);
                response.SetData(Role);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpGet]
        [Route("new/{Name}")]
        public HttpResponseMessage GetRoleByName(string Name)
        {
            IResponseData<Role> response = new ResponseData<Role>();
            try
            {
                Role Role = RoleService.GetRoleByName(Name);
                response.SetData(Role);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("project/{userid}")]
        public HttpResponseMessage GetRoleByUserProject(string userid)
        {
            IResponseData<IEnumerable<Role>> response = new ResponseData<IEnumerable<Role>>();
            try
            {
                IEnumerable<Role> Role;
                if (User.IsSysAdmin())
                {
                    Role = RoleService.GetRoles();
                }
                else
                {
                    Role = RoleService.GetRoleByProjectUser(User.Identity.GetProjectIDSys(), userid);
                }
                response.SetData(Role);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("user")]
        public HttpResponseMessage GetRoleByUserIDProject()
        {
            IResponseData<List<Role>> response = new ResponseData<List<Role>>();
            try
            {
                List<Role> Role = RoleService.GetRoleByUserID(User.Identity.GetUserIdApp());
                response.SetData(Role);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
        // get api/Roles/id

        [HttpGet]
        [Route("{RoleID}")]
        public HttpResponseMessage GetRole(string RoleID)
        {
            IResponseData<Role> response = new ResponseData<Role>();
            try
            {
                Role Role = RoleService.GetRoleByLocIDSys(RoleID);
                response.SetData(Role);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Suppliers
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Role Role)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                //Role.UserUpdate = User.Identity.Name;
                if(Role.ProjectIDSys == 0)
                {
                    Role.ProjectIDSys = User.Identity.GetProjectIDSys();
                }
                string id = RoleService.CreateRole(Role);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Suppliers/5

        [HttpPut]
        [Route("{Id}")]
        public HttpResponseMessage Put(string Id, [FromBody]Role Role)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                if (Role.ProjectIDSys == 0)
                {
                    Role.ProjectIDSys = User.Identity.GetProjectIDSys();
                }
                bool isUpated = RoleService.UpdateRole( Role);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpDelete]
        [Route("{RoleID}")]
        public HttpResponseMessage Delete(string RoleID)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = RoleService.DeleteRole(RoleID);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        private IRoleService GetRoleService()
        {
            return RoleService;
        }


    }
}