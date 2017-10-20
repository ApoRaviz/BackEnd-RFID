using HRMS.Repository.Context;
using HRMS.Repository.Entity.LeaveRequest;
using HRMS.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Status;

namespace HRMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/demo")]
    public class DemoController : ApiController
    {
        private IDemoService DemoService;

        public DemoController(IDemoService demoService)
        {
            DemoService = demoService;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<LeaveDto>> response = new ResponseData<IEnumerable<LeaveDto>>();
            try
            {
                HRMSDbContext hrmsDb = new HRMSDbContext();           

                IEnumerable<LeaveDto> leaves = new List<LeaveDto>();

                leaves = (
                    from l in hrmsDb.Leaves
                    join s in hrmsDb.Status_MT
                    on l.StatusIDSys equals s.StatusIDSys
                    select new LeaveDto
                    {
                        Comment = l.Comment,
                        StatusTitle = s.Title
                    }
                );

                response.SetData(leaves);
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
