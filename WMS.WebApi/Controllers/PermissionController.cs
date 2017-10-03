using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WMS.Master;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Permissions")]
    public class PermissionsController : ApiController
    {
        private IPermissionService PermissionService;
        private IRoleService RoleService;
        private IMenuProjectMappingService MenuProjectMapping;


        public PermissionsController(IPermissionService PermissionService, IRoleService RoleService,IMenuProjectMappingService MenuProjectMappingService)
        {
            this.PermissionService = PermissionService;
            this.RoleService = RoleService;
            this.MenuProjectMapping = MenuProjectMappingService;
        }

        //get api/Permissions
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Permission>> response = new ResponseData<IEnumerable<Permission>>();
            try
            {
                IEnumerable<Permission> Permission = PermissionService.GetPermissions();
                response.SetData(Permission);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("tree/{ProjectIDSys}")]
        public HttpResponseMessage GetTree(int ProjectIDSys)
        {
            ResponseData<List<PermissionTree>> response = new ResponseData<List<PermissionTree>>();
            try
            {
                List<PermissionTree> Permission = PermissionService.GetPermissionTree(ProjectIDSys);
                response.SetData(Permission);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/Permissions/id

        [HttpGet]
        [Route("menu/{MenuIDSys}/{ProjectIDSys}")]
        public HttpResponseMessage GetPermissionMenu(int MenuIDSys,int ProjectIDSys)
        {
            IResponseData<List<Permission>> response = new ResponseData<List<Permission>>();
            try
            {
                List<Permission> Permission = PermissionService.GetPermissionByMenuID(MenuIDSys,ProjectIDSys);
                response.SetData(Permission);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("auto/{MenuIDSys}/{ProjectIDSys}")]
        public HttpResponseMessage GetPermissionMenuAuto(int MenuIDSys, int ProjectIDSys)
        {
            IResponseData<List<Permission>> response = new ResponseData<List<Permission>>();
            try
            {
                List<Permission> Permission = PermissionService.GetPermissionAuto(MenuIDSys, ProjectIDSys);
                response.SetData(Permission);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("role/{RoleID}/{ProjectIDSys}")]
        public HttpResponseMessage GetPermissionByRoleID(string RoleID, int ProjectIDSys)
        {
            IResponseData<List<Permission>> response = new ResponseData<List<Permission>>();
            try
            {
                List<Permission> Permission = PermissionService.GetPermissionByRoleID(RoleID, ProjectIDSys);
                response.SetData(Permission);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("{Id}")]
        public HttpResponseMessage Get(string Id)
        {
            IResponseData<Permission> response = new ResponseData<Permission>();
            try
            {
                Permission Permission = PermissionService.GetPermissionByLocIDSys(Id);
                response.SetData(Permission);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("info/{Id}")]
        public HttpResponseMessage GetPermissionDto(string Id)
        {
            IResponseData<PermissionRoleDto> response = new ResponseData<PermissionRoleDto>();
            PermissionRoleDto responseData = new PermissionRoleDto();
            try
            {
                Permission Permission = PermissionService.GetPermissionByLocIDSys(Id);
                List<RolePermissionDto> Roles = RoleService.GetRoleByPermissionID(Id);
                int ProjectID = 1; // wait for ProjectID
                List<MenuProjectMappingDto> Menu = MenuProjectMapping.GetMenuProjectMappingDto(ProjectID).ToList();
                responseData.PermissionID = Permission.PermissionID;
                responseData.PermissionName = Permission.PermissionName;
                responseData.Roles = Roles;
                response.SetData(responseData);
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
        public HttpResponseMessage Post(PermissionRoleDto Permission)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                //Permission.UserUpdate = User.Identity.Name;
                Permission temp = new Permission()
                {
                    PermissionName = Permission.PermissionName,
                    Method = Permission.Method,
                    MenuIDSys = Permission.MenuProjectMapping[0].MenuIDSys,
                    ProjectIDSys = Permission.MenuProjectMapping[0].ProjectIDSys,
                    ApiIDSys = Permission.ApiIDSys
                };
                string id = PermissionService.CreatePermission(temp);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("rolepermission")]
        public HttpResponseMessage PostPermission(PermissionRoleDto Permission)
        {
            IResponseData<string> response = new ResponseData<string>();
            for (int i = 0; i < Permission.Roles.Count; i++)
            {
                try
                {
                    string id = PermissionService.CreateRolePermission(Permission.PermissionID, Permission.Roles[i].RoleID);
                    response.SetData(id);
                }
                catch (ValidationException ex)
                {
                    response.SetErrors(ex.Errors);
                    response.SetStatus(HttpStatusCode.PreconditionFailed);
                }
            }
            //Permission.UserUpdate = User.Identity.Name;
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("{RoleID}")]
        public HttpResponseMessage PostPermission(string RoleID,List<PermissionTree> Permission)
        {
            IResponseData<string> response = new ResponseData<string>();
                try
                {
                    string id = PermissionService.CreateRolePermission(RoleID, Permission);
                    response.SetData(id);
                }
                catch (ValidationException ex)
                {
                    response.SetErrors(ex.Errors);
                    response.SetStatus(HttpStatusCode.PreconditionFailed);
                }
            //Permission.UserUpdate = User.Identity.Name;
            return Request.ReturnHttpResponseMessage(response);
        }
        // PUT: api/Suppliers/5

        [HttpPut]
        [Route("{Id}")]
        public HttpResponseMessage Put(string Id, [FromBody]Permission Permission)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = PermissionService.UpdatePermission(Id, Permission);
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
        [Route("{PermissionID}/{RoleID}")]
        public HttpResponseMessage DeletePermission(string PermissionID, string RoleID)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = true;
                    isUpated = PermissionService.DeleteRolePermission(PermissionID, RoleID);
                
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
        [Route("{PermissionID}")]
        public HttpResponseMessage DeletePermission2(string PermissionID)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = false;
                isUpated = PermissionService.DeleteAllInRole(PermissionID);
                if(isUpated)
                isUpated = PermissionService.DeletePermission(PermissionID);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        private IPermissionService GetPermissionService()
        {
            return PermissionService;
        }

    }
}
   