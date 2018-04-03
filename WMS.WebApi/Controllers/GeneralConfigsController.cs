
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WMS.Entity.Common;
using WMS.Service.Common;
using Validation = WIM.Core.Common.Utility.Validation;

namespace WMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/GeneralConfig")]
    public class GeneralConfigsController : ApiController
    {
        private IGeneralConfigsService GeneralConfigsService;
        public GeneralConfigsController(IGeneralConfigsService generalConfigsService)
        {
            GeneralConfigsService = generalConfigsService;
        }

        
        [HttpPost]
        [Route("DefaultConfig")]
        public HttpResponseMessage PostDefault([FromBody]GeneralConfig config)
        {
            ResponseData<GeneralConfig> response = new ResponseData<GeneralConfig>();
            try
            {
                GeneralConfig data = GeneralConfigsService.CreateGeneralConfigs(config);
                response.SetData(data);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpGet]
        [Route("{keyword}")]
        public HttpResponseMessage PostDefault(string Keyword)
        {
            ResponseData<GeneralConfig> response = new ResponseData<GeneralConfig>();
            try
            {
                GeneralConfig data = GeneralConfigsService.GetGeneralConfigs(Keyword);
                response.SetData(data);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Create HeadReportControl
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]GeneralConfigsTemplate config)
        {
            ResponseData<GeneralConfig> response = new ResponseData<GeneralConfig>();
            try
            {
                //HeadReportControl headReportControl = new HeadReportControl();

                GeneralConfig Config = new GeneralConfig();
                DetailConfig detail = new DetailConfig();
                detail.Key = config.Key;
                Config.Keyword = config.KeyWord;
                detail.IsReset = config.IsReset;
                detail.Value = config.Value;
                Config.DetailConfig = detail;
                GeneralConfig data = GeneralConfigsService.CreateGeneralConfigs(Config);
                response.SetData(data);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Create HeadReportControl
        [HttpPost]
        [Route("LocationFormat")]
        public HttpResponseMessage saveFormatLocation([FromBody]GeneralConfigLocationFormat config)
        {
            ResponseData<GeneralConfigLocationFormat> response = new ResponseData<GeneralConfigLocationFormat>();
            try
            {
                //HeadReportControl headReportControl = new HeadReportControl();
                
                GeneralConfigLocationFormat data = GeneralConfigsService.saveGeneralConfigLocationFormat(config);
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
        [Route("{config}")]
        public HttpResponseMessage Put([FromBody]GeneralConfigsTemplate config)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                GeneralConfig Config = new GeneralConfig();
                Config.Keyword = config.KeyWord;
                Config.DetailConfig.IsReset = config.IsReset;
                Config.DetailConfig.Key = config.Key;
                bool isUpdated = GeneralConfigsService.UpdateGeneralConfigs(Config);
                response.SetData(isUpdated);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

    }


    public class GeneralConfigsTemplate<T>
    {
        public string KeyWord { get; set; }
        public T DetailConfig { get; set; }
    }

    public class GeneralConfigsFormatlocation
    {
        public string Key { get; set; }
        public string DefaultValue { get; set; }
        public string TypeGen { get; set; }
        public int Digit { get; set; }
    }

    public class GeneralConfigsDefault
    {
        public string KeyWord { get; set; }
        public object DetailConfig { get; set; }
    }

    public class GeneralConfigsTemplate
    {
        public string KeyWord { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string IsReset { get; set; }
    }
}