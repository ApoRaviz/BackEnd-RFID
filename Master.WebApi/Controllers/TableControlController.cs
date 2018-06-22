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
using WIM.Core.Entity.TableControl;
using WIM.Core.Service;

namespace Master.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/TableControl")]
    public class TableControlController : ApiController
    {
        private ITableControlService TableControlService;

        public TableControlController(ITableControlService tableControlService)
        {
            this.TableControlService = tableControlService;
        }

        // GET: api/TableControl
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<TableControl>> response = new ResponseData<IEnumerable<TableControl>>();
            try
            {
                IEnumerable<TableControl> tableControl = TableControlService.GetTableControl();
                response.SetData(tableControl);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


    }
}