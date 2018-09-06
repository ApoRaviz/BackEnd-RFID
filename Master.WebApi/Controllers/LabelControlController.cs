using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Entity.LabelManagement.LabelConfigs;
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
                //Job Comment, ProjectIDSys ไม่ถูก
                //LabelData.ProjectIDSys = (User.IsSysAdmin()) ? LabelData.ProjectIDSys : User.Identity.GetProjectIDSys();

                LabelData.ProjectIDSys = LabelData.ProjectIDSys;
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

        [HttpPost]
        [Route("addlabelconfig/{projectid}")]
        public HttpResponseMessage PostAddLabelConfig([FromUri]int projectid, List<LabelConfig> labelConfig)
        {
            LabelControlService LabelService = new LabelControlService();
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool rs = LabelService.AddLabelConfig(projectid, labelConfig);
                response.SetData(rs);
            }
            catch (NullReferenceException)
            {
                
            }
            return Request.ReturnHttpResponseMessage(response);
        }

    }
}