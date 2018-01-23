using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.Common;
using WIM.Core.Service;
using WMS.Service;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/log")]
    public class LogController : ApiController
    {
        private ICommonService commonService;
        public LogController(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(DateTime? dateFrom, DateTime? dateTo)
        {
            ResponseData<IEnumerable<UserLog>> response = new ResponseData<IEnumerable<UserLog>>();
            try
            {
                IEnumerable<UserLog> logData = commonService.GetUserLogData(null, dateFrom, dateTo);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(logData);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            IResponseData<UserLog> response = new ResponseData<UserLog>();
            try
            {
                UserLog log = commonService.GetUserLogData(id, null, null).FirstOrDefault();
                response.SetData(log);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);

        }

        [HttpGet]
        [Route("Search")]
        public HttpResponseMessage Search(string method, string url, DateTime? dateFrom, DateTime? dateTo)
        {
            ResponseData<IEnumerable<UserLog>> response = new ResponseData<IEnumerable<UserLog>>();
            try
            {
                IEnumerable<UserLog> logData = commonService.GetUserLogData(method, url, dateFrom, dateTo);
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(logData);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);

        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]UserLog log)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isInserted = commonService.WriteUserLog(log);
                response.SetData(isInserted);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
    }
}
