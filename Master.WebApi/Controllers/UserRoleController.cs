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
using WIM.Core.Entity.UserManagement;
using WIM.Core.Service;
using WMS.Service;

namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/UserRoles")]
    public class UserRolesController : ApiController
    {
        private IUserRoleService UserRoleService;
        private IUserService UserService;
        private IRoleService RoleService;

        public UserRolesController(IUserRoleService UserRoleService,IRoleService RoleService,IUserService UserService)
        {
            this.UserService = UserService;
            this.UserRoleService = UserRoleService;
            this.RoleService = RoleService;
        }

        //get api/UserRoles
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<UserRoles>> response = new ResponseData<IEnumerable<UserRoles>>();
            try
            {
                IEnumerable<UserRoles> UserRole = UserRoleService.GetUserRoles();
                response.SetData(UserRole);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/UserRoles/id

        [HttpGet]
        [Route("user/{UserID}")]
        public HttpResponseMessage GetUser(string UserID)
        {
            IResponseData<UserRoleDto> response = new ResponseData<UserRoleDto>();
            try
            {
                UserRoleDto UserRole = UserRoleService.GetUserRoleByUserID(UserID);
                List<RoleUserDto> Roles = UserRoleService.GetRoleByUserID(UserID);
                UserRole.Roles = Roles;
                response.SetData(UserRole);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
        [HttpGet]
        [Route("{RoleID}")]
        public HttpResponseMessage GetUserNotInRole(string RoleID)
        {
            IResponseData<List<User>> response = new ResponseData<List<User>>();
            try
            {
                List<User> users = UserService.getUserNotHave(RoleID);
                response.SetData(users);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/UserRoles/id

        [HttpGet]
        [Route("role/{RoleID}")]
        public HttpResponseMessage GetRole(string RoleID)
        {
            IResponseData<RoleUserDto> response = new ResponseData<RoleUserDto>();
            try
            {
                Role role = RoleService.GetRoleByLocIDSys(RoleID);
                RoleUserDto UserRole = new RoleUserDto();
                UserRole.RoleID = role.RoleID;
                UserRole.Name = role.Name;
                List<UserRoleDto> Users = UserRoleService.GetUserByRoleID(RoleID);
                UserRole.Users = Users;
                response.SetData(UserRole);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/UserRole
        [HttpPost]
        [Route("userrole")]
        public HttpResponseMessage PostUserRole(UserRoleDto UserRole)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                for(int i = 0;i < UserRole.Roles.Count;i++)
                UserRoleService.CreateUserRoles(UserRole.UserID, UserRole.Roles[i].RoleID);
                response.SetData("success");
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("roleuser")]
        public HttpResponseMessage PostRoleUser(RoleUserDto UserRole)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                for (int i = 0; i < UserRole.Users.Count; i++)
                    UserRoleService.CreateRoleUsers(UserRole.Users[i].UserID , UserRole.RoleID);
                response.SetData("success");
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
        [Route("{RoleIDSys}")]
        public HttpResponseMessage Put(int RoleIDSys, [FromBody]UserRoles UserRole)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = UserRoleService.UpdateUserRole( UserRole);
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
        [Route("{UserID}/{RoleID}")]
        public HttpResponseMessage Delete(string UserID , string RoleID)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = UserRoleService.DeleteRolePermission(UserID, RoleID);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        private IUserRoleService GetUserRoleService()
        {
            return UserRoleService;
        }


    }
}