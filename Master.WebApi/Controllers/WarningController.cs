using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.Employee;
using WIM.Core.Service.EmployeeMaster;
using WIM.Core.Service.FileManagement;
using WIM.Core.Service.Impl.FileManagement;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/Warning")]
    public class WarningController : ApiController
    {
        private IHistoryWarningService WarningService;

        public WarningController(IHistoryWarningService warningservice)
        {
            this.WarningService = warningservice;
        }

        // GET: api/Employees
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<HistoryWarning>> response = new ResponseData<IEnumerable<HistoryWarning>>();
            try
            {
                IEnumerable<HistoryWarning> warning = WarningService.GetHistories();
                response.SetData(warning);
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
        [Route("employee/{EmID}")]
        public HttpResponseMessage Get(string EmID)
        {
            IResponseData<IEnumerable<HistoryWarning>> response = new ResponseData<IEnumerable<HistoryWarning>>();
            try
            {
                IEnumerable<HistoryWarning> Employee = WarningService.GetHistoryByEmID(EmID);
                response.SetData(Employee);
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("Download/{fileIDSys}")]
        public HttpResponseMessage GetFile(string fileIDSys)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                new FileService().GetFile("1");
                response.SetData(true);
                
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
        public HttpResponseMessage Post([FromBody]HistoryWarning warning)
        {
            IResponseData<int> response = new ResponseData<int>();
            try
            {
                int id = WarningService.CreateHistory(warning);
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
        public HttpResponseMessage Put([FromBody]HistoryWarning warning)
        {

            IResponseData<bool> response = new ResponseData<bool>();

            try
            {
                bool isUpated = WarningService.UpdateHistory(warning);
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
        [Route("Upload")]
        public async Task<HttpResponseMessage> PostUploadFile()
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                HttpRequestMessage request = Request;
                if (!request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                DateTime d = DateTime.Now;
                string root = @"D:\Uploads\WIM\Warning";//HttpContext.Current.Server.MapPath("~/Handy/Upload");
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                var provider = new MultipartFormDataStreamProvider(root);
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.FileData)
                {
                    //var fileName = Request.Headers.GetValues("filename").FirstOrDefault();
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    string filePath = System.IO.Path.GetDirectoryName(file.LocalFileName) + "\\" + fileName;
                    System.IO.File.Copy(file.LocalFileName, filePath);
                    System.IO.File.Delete(file.LocalFileName);
                }

                response.SetData("success");
            }
            catch (ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        // DELETE: api/Employees/5
        //[HttpDelete]
        //[Route("{DepID}")]
        //public HttpResponseMessage Delete(int DepID)
        //{
        //    IResponseData<bool> response = new ResponseData<bool>();
        //    try
        //    {
        //        bool isUpated = DepartmentService.DeleteDepartment(DepID);
        //        response.SetData(isUpated);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        response.SetErrors(ex.Errors);
        //        response.SetStatus(HttpStatusCode.PreconditionFailed);
        //    }
        //    return Request.ReturnHttpResponseMessage(response);
        //}

    }
}