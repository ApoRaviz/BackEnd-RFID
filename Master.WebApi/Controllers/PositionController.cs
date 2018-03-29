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
using WIM.Core.Entity.Employee;
using WIM.Core.Service.EmployeeMaster;

namespace Master.WebApi
{
    [RoutePrefix("api/v1/Position")]
    public class PositionController : ApiController
    {
        private IPositionService PositionService;

        public PositionController(IPositionService Positionservice)
        {
            this.PositionService = Positionservice;
        }

        // GET: api/Employees
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<Positions>> response = new ResponseData<IEnumerable<Positions>>();
            try
            {
                IEnumerable<Positions> Employees = PositionService.GetPositions();
                response.SetData(Employees);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // GET: api/Employees/1
        [HttpGet]
        [Route("{DepIDSys}")]
        public HttpResponseMessage Get(int DepIDSys)
        {
            IResponseData<Positions> response = new ResponseData<Positions>();
            try
            {
                Positions Employee = PositionService.GetPositionByPositionIDSys(DepIDSys);
                response.SetData(Employee);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST: api/Employees
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Positions Position)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                Position.UpdateBy = User.Identity.Name;
                int id = PositionService.CreatePosition(Position);
                response.SetData(id);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Employees/5

        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put([FromBody]Positions Position)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = PositionService.UpdatePosition(Position);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Employees/5
        [HttpDelete]
        [Route("{DepID}")]
        public HttpResponseMessage Delete(int DepID)
        {
            IResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpated = PositionService.DeletePosition(DepID);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("positionconfig/{id}")]

        public HttpResponseMessage setPositionConfig(int id,[FromBody]  List<PositionConfig<List<PositionConfig<string>>>> positionConfig)
        {
            IResponseData<Positions> response = new ResponseData<Positions>();
            try
            {
                Positions rs = PositionService.SetPositionConfig(id,positionConfig);
                response.SetData(rs);
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