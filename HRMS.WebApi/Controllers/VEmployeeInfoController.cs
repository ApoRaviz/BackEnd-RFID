using HRMS.Entity.Probation;
using HRMS.Service.Probation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;

namespace HRMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/Probation")]
    public class VEmployeeInfoController : ApiController
    {
        private IVEmployeeInfoService VEmployeeInfoService;

        public VEmployeeInfoController(IVEmployeeInfoService VEmployeeInfoService)
        {
            this.VEmployeeInfoService = VEmployeeInfoService;

        }

        [HttpGet]
        [Route("list")]
        public HttpResponseMessage GetList()
        {
            ResponseData<IEnumerable<VEmployeeInfo>> response = new ResponseData<IEnumerable<VEmployeeInfo>>();
            try
            {
                IEnumerable<VEmployeeInfo> VEmployeeInfo = VEmployeeInfoService.GetProbation();
                response.SetData(VEmployeeInfo);
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