using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WIM.Core.Common;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;
using System.Data.Entity;

namespace Fuji.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/helpers")]
    public class HelpersController : ApiController
    {

        private ICommonService CommonService;
        public HelpersController(ICommonService commonService)
        {
            this.CommonService = commonService;
        }

        // GET: api/Helpers
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("tableDescription/{tableName}")]
        public HttpResponseMessage TableDescription(string tableName)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                string tableDescription = CommonService.GetTableDescription(tableName);
                response.SetData(tableDescription);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("tableColumnsDescription/{tableName}")]
        public HttpResponseMessage TableColumnsDescription(string tableName)
        {
            IResponseData<IEnumerable<ProcGetTableColumnsDescription_Result>> response = new ResponseData<IEnumerable<ProcGetTableColumnsDescription_Result>>();
            try
            {
                IList<ProcGetTableColumnsDescription_Result> tableColsDescription = CommonService.GetTableColumnsDescription(tableName);
                response.SetData(tableColsDescription);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("HttpStatusCode/{httpStatusCode}")]
        public HttpResponseMessage GetHttpStatusCode(int httpStatusCode)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                response.SetData("");
                throw new ValidationException("Key_1", "Error 1");
            }
            catch (ValidationException ex)
            {
                ex.Add(new ValidationError("Key_2", "Error 2", null));
                response.SetErrors(ex.Errors);
                response.SetStatus((HttpStatusCode)httpStatusCode);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetData")]
        public HttpResponseMessage GetData()
        {
            //Master.MasterContext db = Master.MasterContext.Create();
            //IResponseData<List<WMS.Master.Role>> response = new ResponseData<List<WMS.Master.Role>>();
            //var roles = (from r in db.Roles
            //             select r
            //             )
            //             .Include(r => r.Permissions).ToList();

            IResponseData<string> response = new ResponseData<string>();
            response.SetData(Request.GetHeaderValue("Accept"));
            response.SetStatus(HttpStatusCode.OK);
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpGet]
        [Route("autocomplete")]
        public HttpResponseMessage GetDataAutoComplete(string columnNames, string tableName, string conditionColumnNames, string keyword)
        {
            IResponseData<string> response = new ResponseData<string>();
            if (string.IsNullOrEmpty(keyword))
            {
                response.SetData(null);
                Request.ReturnHttpResponseMessage(response);
            }

            try
            {
                string result = CommonService.GetDataAutoComplete(columnNames, tableName, conditionColumnNames, keyword);
                response.SetData(result);
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
