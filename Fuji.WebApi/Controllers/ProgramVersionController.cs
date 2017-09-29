using Fuji.Repository;
using Fuji.Service.ProgramVersion;
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
    [RoutePrefix("api/v1/external/programVersion")]
    public class ProgramVersionController : ApiController
    {
        private IProgramVersionService ProgramVersionService;
        public ProgramVersionController(IProgramVersionService ProgramVersionService)
        {
            this.ProgramVersionService = ProgramVersionService;
        }

        [HttpGet]
        [Route("{programName}")]
        public HttpResponseMessage Get(string programName)
        {
            IResponseData<ProgramVersionHistory> response = new ResponseData<ProgramVersionHistory>();
            try
            {
                //int baseRunning = ProgramVersionService.GetRunningByType(type, running);
                ProgramVersionHistory version = ProgramVersionService.GetProgramVersion(programName);
                response.SetData(version);
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
