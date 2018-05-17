using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;
using WMS.Context;
using WIM.Core.Common.ValueObject;
using WIM.Core.Service;
using WIM.Core.Service.Impl;
using WMS.Common.ValueObject;

namespace WMS.WebApi.Controller
{
    [Authorize]
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
                string tableDescription = new WMSDbContext().GetTableDescriptionWms(tableName);
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
            IResponseData<IEnumerable<TableColumnsDescription>> response = new ResponseData<IEnumerable<TableColumnsDescription>>();
            try
            {
                IEnumerable<TableColumnsDescription> tableColsDescription = new WMSDbContext().GetTableColumnsDescription(tableName);
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
        [Route("validationField/{tableName}")]
        public HttpResponseMessage ValidationField(string tableName)
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                string tableColsDescription = new WMSDbContext().GetValidation(tableName);
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
                WMSDbContext db = new WMSDbContext();
                string result = db.ProcGetDataAutoComplete(columnNames, tableName, conditionColumnNames, keyword);
                response.SetData(result);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("generate/{keyword}")]
        public HttpResponseMessage GetKeyGenerator(string keyword)
        {
            IResponseData<string> response = new ResponseData<string>();
            if (string.IsNullOrEmpty(keyword))
            {
                response.SetData(null);
                Request.ReturnHttpResponseMessage(response);
            }

            try
            {
                ICommonService common = new CommonService();
                string result = common.GetValueGenerateCode(keyword);
                response.SetData(result);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("checkdependentPK")]
        public HttpResponseMessage CheckDependentPK(string TableName, string ColumnName, string Value = "")
        {
            IResponseData<IEnumerable<CheckDependentPKDto>> response = new ResponseData<IEnumerable<CheckDependentPKDto>>();
            if (string.IsNullOrEmpty(TableName) || string.IsNullOrEmpty(ColumnName))
            {
                response.SetData(null);
                Request.ReturnHttpResponseMessage(response);
            }

            try
            {
                Service.Common.ICommonService commonWMS = new Service.Impl.Common.CommonService();
                IEnumerable<CheckDependentPKDto> result = new List<CheckDependentPKDto>();
                result = commonWMS.CheckDependentPK(TableName, ColumnName, Value);
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
