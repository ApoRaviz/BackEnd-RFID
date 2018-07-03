using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WMS.Entity.Dimension;
using WMS.Service;

namespace WMS.WebApi.Controller
{
    //[Authorize]
    [RoutePrefix("api/v1/dimension")]
    public class DimensionController : ApiController
    {
        private IDimensionService DimensionService;

        public DimensionController(IDimensionService DimensionService)
        {
            this.DimensionService = DimensionService;
        }

        [HttpGet]
        [Route("GetAllDimension")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<DimensionLayout_MT>> response = new ResponseData<IEnumerable<DimensionLayout_MT>>();
            try
            {
                IEnumerable<DimensionLayout_MT> data = DimensionService.GetAllDimension();
                response.SetData(data);
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
        [Route("{DimensionIDSys}")]
        public HttpResponseMessage Get(int DimensionIDSys)
        {
            IResponseData<DimensionLayout_MT> response = new ResponseData<DimensionLayout_MT>();
            try
            {
                DimensionLayout_MT data = DimensionService.GetDimensionLayoutByDimensionIDSys(DimensionIDSys);
                response.SetData(data);
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
        [Route("GetColor/{DimensionIDSys}")]
        public HttpResponseMessage GetColor(int? DimensionIDSys)
        {
            IResponseData<List<string>> response = new ResponseData<List<string>>();
            try
            {
                List<string> data = DimensionService.GetColorInSystem(DimensionIDSys);
                response.SetData(data);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }
        
        [HttpGet]
        [Route("GetBlock")]
        public HttpResponseMessage GetBlock()
        {
            IResponseData<List<DimensionLayout_MT>> response = new ResponseData<List<DimensionLayout_MT>>();
            try
            {
                List<DimensionLayout_MT> data = DimensionService.GetBlock();
                response.SetData(data);
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
        public HttpResponseMessage Post([FromBody]DimensionLayout_MT data)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                data.UpdateBy = User.Identity.Name;
                int id = DimensionService.CreateDimensionOfLocation(data).Value;            
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
        [Route("{DimensionIDSys}")]
        public HttpResponseMessage Put(int DimensionIDSys, [FromBody]DimensionLayout_MT data)
        {
            IResponseData<int> response = new ResponseData<int>();

            try
            {
                int isUpated = DimensionService.UpdateDimensionOfLocation(DimensionIDSys, data).Value;
                response.SetData(isUpated);
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
