using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common;
using System.Web.Http.Cors;
using WIM.Core.Entity.Country;
using WIM.Core.Service;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Service.PermissionGroups;
using WIM.Core.Entity.MenuManagement;

namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/permissiongroup")]
    public class PermissionGroupController : ApiController
    {

        private IPermissionGroupService GroupService;

        public PermissionGroupController(IPermissionGroupService groupService)
        {
            this.GroupService = groupService;
        }

        //get api/Countrys
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<PermissionGroup>> response = new ResponseData<IEnumerable<PermissionGroup>>();
            try
            {
                IEnumerable<PermissionGroup> Group = GroupService.GetPermissionGroup();
                response.SetData(Group);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // get api/Countrys/id

        [HttpGet]
        [Route("{GroupIDSys}")]
        public HttpResponseMessage Get(string GroupIDSys)
        {
            IResponseData<PermissionGroup> response = new ResponseData<PermissionGroup>();
            try
            {
                PermissionGroup Group = GroupService.GetGroupByGroupIDSys(GroupIDSys);
                response.SetData(Group);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("menu/{MenuIDSys}")]
        public HttpResponseMessage Menu(int MenuIDSys)
        {
            IResponseData<IEnumerable<PermissionGroup>> response = new ResponseData<IEnumerable<PermissionGroup>>();
            try
            {
                IEnumerable<PermissionGroup> Group = GroupService.GetGroupByMenuIDSys(MenuIDSys);
                response.SetData(Group);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        /// <summary>
        /// For New Group and set of api
        /// </summary>
        /// <param name="Group"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]IEnumerable<PermissionGroup> Group)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool id = GroupService.CreateGroup(Group);
                response.SetData(id);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpPost]
        [Route("newApi")]
        public HttpResponseMessage PostNewApi([FromBody]IEnumerable<PermissionGroupApi> Group)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool id = GroupService.CreateApi(Group);
                response.SetData(id);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
        // PUT: api/PermissionGroup/5

        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put( [FromBody]PermissionGroup group)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = GroupService.UpdateGroup( group);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpDelete]
        [Route("{CountryIDSys}")]
        public HttpResponseMessage Delete(int CountryIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                //bool isUpdated = CountryService.DeleteCountry(CountryIDSys);
                response.SetData(true);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


    }

}
