using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Validation;
using WIM.Core.Common.Extensions;
using WIM.Core.Common.Http;
using WMS.Common;
using WMS.Service;
using WMS.Service.Import;
using WMS.Entity.ImportManagement;

namespace WMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/import")]
    public class ImportController : ApiController
    {
        private IImportService ImportService;

        public ImportController(IImportService importService)
        {
            this.ImportService = importService;
        }

        [HttpGet]
        [Route("GetHeader/{ForTable}")]
        public HttpResponseMessage Get(string forTable)
        {
            ResponseData<IEnumerable<ImportDefinitionHeader_MT>> response = new ResponseData<IEnumerable<ImportDefinitionHeader_MT>>();
            try
            {
                IEnumerable<ImportDefinitionHeader_MT> header = ImportService.GetAllImportHeader(forTable);
                response.SetData(header);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/label/1
        [HttpGet]
        [Route("{ImportIDSys}")]
        public HttpResponseMessage Get(int ImportIDSys)
        {
            IResponseData<ImportDefinitionHeader_MT> response = new ResponseData<ImportDefinitionHeader_MT>();
            try
            {
                ImportDefinitionHeader_MT import = ImportService.GetImportDefinitionByImportIDSys(ImportIDSys, "ImportDefinitionDetail_MT");
                response.SetData(import);
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
        public HttpResponseMessage Post([FromBody]ImportDefinitionHeader_MT data)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                data.UpdateBy = User.Identity.Name;
                int id = ImportService.CreateImportDifinitionForItemMaster(data).Value;
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPut]
        [Route("{ImportIDSys}")]
        public HttpResponseMessage Put(int ImportIDSys, [FromBody]ImportDefinitionHeader_MT data)
        {
            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = ImportService.UpdateImportForItemMaster(ImportIDSys, data);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpDelete]
        [Route("{ImportIDSys}")]
        public HttpResponseMessage Delete(int ImportIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = ImportService.DeleteImport(ImportIDSys);
                response.SetData(isUpated);
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
