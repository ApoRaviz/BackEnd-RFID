using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;

namespace Fuji.WebApi.Controllers
{
    [RoutePrefix("api/v1/handyhelpers")]
    public class HandyHelpersController : ApiController
    {

        private IHelperService HelperService;
        public HandyHelpersController(IHelperService helperService)
        {
            HelperService = helperService;
        }

        [HttpPost]
        [Route("errorLog")]
        public HttpResponseMessage PostErrorLog([FromBody]ErrorLogs errorLog)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                HelperService.InsertErrorLog(errorLog);
                response.SetData(1);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.OK);
            }
            return Request.ReturnHttpResponseMessage(response);
            
        }


    }
}
