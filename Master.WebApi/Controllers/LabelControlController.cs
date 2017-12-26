using Master.Common.ValueObject.LabelControl;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Service;
using WIM.Core.Service.Impl;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/labelcontrol")]
    public class LabelControlController : ApiController
    {
        [HttpGet]
        [Route("{lang}/{projectid}")]
        public HttpResponseMessage GetDto(string Lang,int ProjectID)
        {
            ILabelControlService  LabelService = new LabelControlService();
            LabelControlDto labelResponse = new LabelControlDto();
            ResponseData<LabelControlDto> response = new ResponseData<LabelControlDto>();


            try
            {
                ProjectID = (User.IsSysAdmin()) ? ProjectID : User.Identity.GetProjectIDSys();
                labelResponse = LabelService.GetDto(Lang, ProjectID);
                response.SetData(labelResponse);
                }
                catch (NullReferenceException)
                {

                }
            
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]LabelControl LabelData)
        {
            LabelControlService LabelService = new LabelControlService();
            LabelControlDto labelResponse = new LabelControlDto();
            ResponseData<LabelControlDto> response = new ResponseData<LabelControlDto>();
            try
            {
                LabelData.ProjectIDSys = (User.IsSysAdmin()) ? LabelData.ProjectIDSys : User.Identity.GetProjectIDSys();
                labelResponse = LabelService.CreateLabelControl(LabelData);
                response.SetData(labelResponse);
            }
            catch (NullReferenceException)
            {

            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put([FromBody]LabelControl LabelData)
        {
            LabelControlService LabelService = new LabelControlService();
            LabelControlDto labelResponse = new LabelControlDto();
            ResponseData<LabelControlDto> response = new ResponseData<LabelControlDto>();
            try
            {
                LabelData.ProjectIDSys = (User.IsSysAdmin()) ? LabelData.ProjectIDSys : User.Identity.GetProjectIDSys();
                labelResponse = LabelService.UpdateLabelControl(LabelData);
                response.SetData(labelResponse);
            }
            catch (NullReferenceException)
            {

            }
            return Request.ReturnHttpResponseMessage(response);
        }

    }
}