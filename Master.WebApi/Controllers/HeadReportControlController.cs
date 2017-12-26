using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Validation = WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Service;
using WIM.Core.Service.Impl;
using WIM.Core.Entity.LabelManagement;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/HeadReportControl")]
    public class HeadReportControlController : ApiController
    {
        private IHeadReportControlService HeadReportControlService;
        public HeadReportControlController(IHeadReportControlService headReportControlService)
        {
            HeadReportControlService = headReportControlService;
        }

        //Create HeadReportControl
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]HeadReportControl HeadReportConfig)
        {
            ResponseData<HeadReportControl> response = new ResponseData<HeadReportControl>();
            try
            {
                //HeadReportControl headReportControl = new HeadReportControl();

                int id = HeadReportControlService.CreateHeadReportControl(HeadReportConfig);
                HeadReportControl data = HeadReportControlService.GetHeadReportControlByID(id);
                response.SetData(data);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Update HeadReportControl
        [HttpPut]
        [Route("{HeadReportControlIDSys}")]
        public HttpResponseMessage Put([FromBody]HeadReportControl HeadReportControlUpdate)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpdated = HeadReportControlService.UpdateHeadReportControl(HeadReportControlUpdate);
                response.SetData(isUpdated);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Get HeadReportControl By ID
        [HttpGet]
        [Route("submodule/{SubModuleIDSys}")]
        public HttpResponseMessage Getsubmodule(int SubModuleIDSys)
        {
            ResponseData<IEnumerable<HeadReportControl>> response = new ResponseData<IEnumerable<HeadReportControl>>();
            try
            {
                IEnumerable<HeadReportControl> HeadReportControlByID = HeadReportControlService.GetHeadReportControlsByModuleID(SubModuleIDSys);
                response.SetData(HeadReportControlByID);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Get HeadReportControl By ID
        [HttpGet]
        [Route("{HeadReportControlIDSys}")]
        public HttpResponseMessage Get(int HeadReportControlIDSys)
        {
            ResponseData<HeadReportControl> response = new ResponseData<HeadReportControl>();
            try
            {
                HeadReportControl HeadReportControlByID = HeadReportControlService.GetHeadReportControlByID(HeadReportControlIDSys);
                response.SetData(HeadReportControlByID);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Get Data
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<HeadReportControl>> response = new ResponseData<IEnumerable<HeadReportControl>>();
            try
            {
                IEnumerable<HeadReportControl> HeadReportControl = HeadReportControlService.GetHeadReportControls();
                response.SetData(HeadReportControl);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

    }
}