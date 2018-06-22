using HRMS.Common.ValueObject;
using HRMS.Common.ValueObject.ReportEvaluation;
using HRMS.Entity.Evaluate;
using HRMS.Entity.Form;
using HRMS.Service.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        [Route("getFormQuestion/{id}")]
        public HttpResponseMessage Get(int id)
        {
            ResponseData<IEnumerable<FormQuestion>> response = new ResponseData<IEnumerable<FormQuestion>>();
            try
            {
                IEnumerable<FormQuestion> FormQuestion = FormService.GetFormQuestionByFormTopicID(id);
                response.SetData(FormQuestion);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("getFormDetail/{EvaluatedIDSys}")]
        public HttpResponseMessage Get1(int EvaluatedIDSys)
        {
            ResponseData<IEnumerable<FormDetail>> response = new ResponseData<IEnumerable<FormDetail>>();
            try
            {
                IEnumerable<FormDetail> FormDetail = FormService.GetFormDetailByEvaID(EvaluatedIDSys);
                response.SetData(FormDetail);
            }
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
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
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [HttpGet]
        [Route("getEvaluatedForm/{id}")]
        public HttpResponseMessage GetHeaderPDF(int id)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            //ResponseData<IEnumerable<object>> response = new ResponseData<IEnumerable<object>>();
            try
            {
                IEnumerable<EvaluatedReport> item1 = FormService.GetEvaluatedFormByID(id);
                IEnumerable<EvaluationTable> item2 = FormService.GetEvaluatedFormDetailByID(id);
                //response.SetData(item);
                if (item1 != null && item2 != null)
                {
                    result.Content = FormService.GetReportStream(item1, item2);
                }
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            }
            catch (AppValidationException ex)
            {
                result = Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
                //response.SetErrors(ex.Errors);
                //response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return result;
            //return Request.ReturnHttpResponseMessage(response);
        }



        // PUT: api/Employees/5

        [HttpPut]
        [Route("FormDetail")]
        public HttpResponseMessage Put([FromBody]IEnumerable<FormDetail> formDetail)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = FormService.UpdateFormDetail(formDetail);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }

            return Request.ReturnHttpResponseMessage(response);
        }

        // PUT: api/Employees/5

        [HttpPut]
        [Route("Evaluated")]
        public HttpResponseMessage Put([FromBody]Evaluated evaluated)
        {

            IResponseData<Evaluated> response = new ResponseData<Evaluated>();

            try
            {
                Evaluated isUpated = FormService.UpdateEvaluated(evaluated);
                response.SetData(isUpated);
            }
            catch (AppValidationException ex)
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