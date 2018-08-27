using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WMS.Service;
using WMS.Service.Import;
using WMS.Entity.ImportManagement;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Extensions;
using System.IO;
using Newtonsoft.Json;
using System.Web;
using WMS.WebApi.ServiceBus.IntegrationEvents;

namespace WMS.WebApi.Controller
{
    [RoutePrefix("api/v1/import")]
    public class ImportController : ApiController
    {
        private IImportService ImportService;

        public ImportController(IImportService importService)
        {
            this.ImportService = importService;
        }

        [HttpPost]
        [Route("GetHeader")]
        public HttpResponseMessage GetHeader(GetHeaderParam getHeader)
        {
            ResponseData<IEnumerable<ImportDefinitionHeader_MT>> response = new ResponseData<IEnumerable<ImportDefinitionHeader_MT>>();
            try
            {
                if(getHeader != null)
                { 
                    IEnumerable<ImportDefinitionHeader_MT> header = ImportService.GetAllImportHeader(getHeader.ProjectId,getHeader.ForTable);
                    response.SetData(header);
                }
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("GetDefColDetail/{Table}")]
        public HttpResponseMessage GetDefinitionColumnDetail(string Table)
        {
            ResponseData<object> response = new ResponseData<object>();
            try
            {
                object items;
                string serverpath = HttpContext.Current.Server.MapPath("~/Config/Definition/" + Table + "-Def.json"); 
                using (StreamReader r = new StreamReader(serverpath))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<object>(json);
                }

                response.SetData(items);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
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
                data.CreateBy = User.Identity.Name;
                int id = ImportService.CreateImportDifinitionForItemMaster(data).Value;
                response.SetData(id);
            }
            catch (AppValidationException ex)
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
                data.UpdateBy = User.Identity.Name;
                bool isUpated = ImportService.UpdateImportForItemMaster(ImportIDSys, data);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("GetReceiveManualImportDefinition")]
        public HttpResponseMessage GetReceiveImportDefinition(ReceiveManualImportIntegrationEvent @event)
        {
            var def = ImportService.GetImportDefinitionByImportIDSys(@event.FormatId, "ImportDefinitionDetail_MT");
            def.ImportDefinitionDetail_MT = (from p in def.ImportDefinitionDetail_MT
                                             select new ImportDefinitionDetail_MT()
                                             {
                                                 ImportDefHeadIDSys = p.ImportDefHeadIDSys,
                                                 ImportDefDetailIDSys = p.ImportDefDetailIDSys,
                                                 ColumnName = p.ColumnName,
                                                 Digits = p.Digits,
                                                 DataType = p.DataType,
                                                 FixedValue = p.FixedValue,
                                                 Import = p.Import,
                                                 IsActive = p.IsActive,
                                                 CreateAt = p.CreateAt,
                                                 CreateBy = p.CreateBy,
                                                 IsHead = p.IsHead,
                                                 IsRefKey = p.IsRefKey,
                                                 Mandatory = p.Mandatory,
                                                 UpdateAt = p.UpdateAt,
                                                 UpdateBy = p.UpdateBy
                                             }).ToList();

            ReceiveManualImportReplyIntegrationEvent ret = new ReceiveManualImportReplyIntegrationEvent(@event.FileIds, def);

            return Request.CreateResponse(ret);
        }

        public class GetHeaderParam
        {
            public string ForTable { get; set; }
            public int ProjectId { get; set; }
        }
    }
}
