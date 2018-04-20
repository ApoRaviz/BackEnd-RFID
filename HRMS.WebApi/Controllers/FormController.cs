using HRMS.Entity.Evaluate;
using HRMS.Entity.Form;
using HRMS.Service.Form;
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

namespace HRMS.WebApi.Controllers
{
    [RoutePrefix("api/v1/Evaluate")]
    public class FormController : ApiController
    {
        private IFormService FormService;

        public FormController(IFormService Formservice)
        {
            this.FormService = Formservice;
        }

        // GET: api/Form
        [HttpGet]
        [Route("getFormQuestion")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<FormQuestion>> response = new ResponseData<IEnumerable<FormQuestion>>();
            try
            {
                IEnumerable<FormQuestion> FormQuestion = FormService.GetFormQuestion();
                response.SetData(FormQuestion);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("getFormDetail/{EvaluatedIDSys}")]
        public HttpResponseMessage Get(int EvaluatedIDSys)
        {
            ResponseData<IEnumerable<FormDetail>> response = new ResponseData<IEnumerable<FormDetail>>();
            try
            {
                IEnumerable<FormDetail> FormDetail = FormService.GetFormDetailByEvaID(EvaluatedIDSys);
                response.SetData(FormDetail);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("getEvaluated/{EvaluatedIDSys}")]
        public HttpResponseMessage Get2(int EvaluatedIDSys)
        {
            ResponseData<Evaluated> response = new ResponseData<Evaluated>();
            try
            {
                Evaluated evaluated = FormService.GetEvaluatedByEvaID(EvaluatedIDSys);
                response.SetData(evaluated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("getEvaluated")]
        public HttpResponseMessage Get2()
        {
            ResponseData<IEnumerable<object>> response = new ResponseData<IEnumerable<object>>();
            try
            {
                IEnumerable<object> evaluated = FormService.GetEvaluated();
                response.SetData(evaluated);
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
        [Route("FormDetail")]
        public HttpResponseMessage Put([FromBody]IEnumerable<FormDetail> formDetail )
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = FormService.UpdateFormDetail(formDetail);
                response.SetData(isUpated);
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
        [Route("Evaluated")]
        public HttpResponseMessage Put([FromBody]Evaluated evaluated )
        {

            IResponseData<Evaluated> response = new ResponseData<Evaluated>();

            try
            {
                Evaluated isUpated = FormService.UpdateEvaluated(evaluated);
                response.SetData(isUpated);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        //    // DELETE: api/Employees/5
        //    [HttpDelete]
        //    [Route("{SpfIDSys}")]
        //    public HttpResponseMessage Delete(int SpfIDSys)
        //    {
        //        IResponseData<bool> response = new ResponseData<bool>();
        //        try
        //        {
        //            bool isUpated = FormService.DeleteForm(SpfIDSys);
        //            response.SetData(isUpated);
        //        }
        //        catch (ValidationException ex)
        //        {
        //            response.SetErrors(ex.Errors);
        //            response.SetStatus(HttpStatusCode.PreconditionFailed);
        //        }
        //        return Request.ReturnHttpResponseMessage(response);
        //    }

    }

}