using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.ValueObject;
using WMS.Common.ValueObject;
using WMS.Entity.ItemManagement;
using WMS.Service;

namespace WMS.WebApi.Controller
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
            ResponseData<IEnumerable<Unit_MT>> response = new ResponseData<IEnumerable<Unit_MT>>();
            try
            {
                IEnumerable<Unit_MT> units = UnitService.GetUnits();
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
            IResponseData<Unit_MT> response = new ResponseData<Unit_MT>();
            try
            {
                Unit_MT unit = UnitService.GetUnitByUnitIDSys(unitIDSys);
                response.SetData(unit);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);

        }

        [HttpGet]
        [Route("autocomplete/{term}")]
        public HttpResponseMessage AutocompleteUnit(string term)
        {
            IResponseData<IEnumerable<AutocompleteUnitDto>> response = new ResponseData<IEnumerable<AutocompleteUnitDto>>();
            try
            {
                if (string.IsNullOrEmpty(term))
                {
                    throw new Exception("Missing term");
                }
                IEnumerable<AutocompleteUnitDto> unit = UnitService.AutocompleteUnit(term);
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
                unit.ProjectIDSys = User.Identity.GetProjectIDSys();
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
                bool isUpated = UnitService.UpdateUnit(unit);
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
