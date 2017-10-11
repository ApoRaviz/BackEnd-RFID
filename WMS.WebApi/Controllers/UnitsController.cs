using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Extensions;
using WMS.Master;
using WMS.Master.Unit;
using WIM.Core.Common.Http;
using WIM.Core.Common.Validation;

namespace WMS.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/units")]
    public class UnitsController : ApiController
    {

        private IUnitService UnitService;
        public UnitsController(IUnitService unitService)
        {
            this.UnitService = unitService;
        }

        // GET: api/Units
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<ProcGetUnits_Result>> response = new ResponseData<IEnumerable<ProcGetUnits_Result>>();
            try
            {
                IEnumerable<ProcGetUnits_Result> units = UnitService.GetUnits();
                response.SetStatus(HttpStatusCode.OK);
                response.SetData(units);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Units/1
        [HttpGet]
        [Route("{unitIDSys}")]
        public HttpResponseMessage Get(int unitIDSys)
        {
            IResponseData<ProcGetUnitByUnitIDSys_Result> response = new ResponseData<ProcGetUnitByUnitIDSys_Result>();
            try
            {
                ProcGetUnitByUnitIDSys_Result unit = UnitService.GetUnitByUnitIDSys(unitIDSys);
                response.SetData(unit);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);

        }

        // POST: api/Units
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Unit_MT unit)
        {
            IResponseData<Unit_MT> response = new ResponseData<Unit_MT>();
            try
            {
                unit.UnitID = "1";
                unit.Active = 1;
                unit.ProjectIDSys = User.Identity.GetProjectIDSys();
                unit.UserUpdate = User.Identity.Name;
                int id = UnitService.CreateUnit(unit);
                unit.UnitIDSys = id;
                response.SetData(unit);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Units/5
        [HttpPut]
        [Route("{unitIDSys}")]
        public HttpResponseMessage Put(int unitIDSys, [FromBody]Unit_MT unit)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = UnitService.UpdateUnit(unitIDSys, unit);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Units/5
        [HttpDelete]
        [Route("{unitIDSys}")]
        public HttpResponseMessage Delete(int unitIDSys)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = UnitService.DeleteUnit(unitIDSys);
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

    class UnitsModel
    {
        public int UnitIDSys { get; set; }
        public string UnitName { get; set; }
         
    }
}
