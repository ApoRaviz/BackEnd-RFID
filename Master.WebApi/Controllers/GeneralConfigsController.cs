﻿using System.Collections.Generic;
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
using WIM.Core.Service.Common;
using WIM.Core.Entity.Common;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/GeneralConfig")]
    public class GeneralConfigsController : ApiController
    {
        private IGeneralConfigsService GeneralConfigsService;
        public GeneralConfigsController(IGeneralConfigsService generalConfigsService)
        {
            GeneralConfigsService = generalConfigsService;
        }

        //Create HeadReportControl
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]GeneralConfigsTemplate config)
        {
            ResponseData<GeneralConfigs> response = new ResponseData<GeneralConfigs>();
            try
            {
                //HeadReportControl headReportControl = new HeadReportControl();

                GeneralConfigs Config = new GeneralConfigs();
                DetailConfig detail = new DetailConfig();
                detail.Key = config.Key;
                Config.Keyword = config.KeyWord;
                detail.IsReset = config.IsReset;
                detail.Value = config.Value;
                Config.DetailConfig = detail;
                GeneralConfigs data = GeneralConfigsService.CreateGeneralConfigs(Config);
                response.SetData(data);
            }
            catch (Validation.AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Update HeadReportControl
        [HttpPut]
        [Route("{config}")]
        public HttpResponseMessage Put([FromBody]GeneralConfigsTemplate config)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                GeneralConfigs Config = new GeneralConfigs();
                Config.Keyword = config.KeyWord;
                Config.DetailConfig.IsReset = config.IsReset;
                Config.DetailConfig.Key = config.Key;
                bool isUpdated = GeneralConfigsService.UpdateGeneralConfigs(Config);
                response.SetData(isUpdated);
            }
            catch (Validation.AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

    }

    public class GeneralConfigsTemplate
    {
        public string KeyWord { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string IsReset { get; set; }
    }
}