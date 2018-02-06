using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Validation = WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.Status;
using WIM.Core.Common.ValueObject;
using WIM.Core.Service.StatusManagement;
using WIM.Core.Service.Impl.StatusManagement;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Service.FileManagement;
using WIM.Core.Service.Impl.FileManagement;
using WIM.Core.Entity.FileManagement;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/File")]
    public class FileController : ApiController
    {
        private IFileService fileService;
        public FileController(IFileService statusService)
        {
            fileService = new FileService();
        }

        //Create Status
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Post()
        {
            ResponseData<IEnumerable<File_MT>> response = new ResponseData<IEnumerable<File_MT>>();
            try
            {
                HttpRequestMessage request = Request;
                if (!request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                DateTime d = DateTime.Now;
                string root = @"D:\Uploads\WIM";//HttpContext.Current.Server.MapPath("~/Handy/Upload");
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                var provider = new MultipartFormDataStreamProvider(root);
                await Request.Content.ReadAsMultipartAsync(provider);
                List<File_MT> files = new List<File_MT>();
                foreach (var file in provider.FileData)
                {
                    File_MT filetemp = new File_MT();
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    string[] temp = fileName.Split('.');
                    string extension = temp[temp.Length - 1];
                    filetemp.FileName = fileName;
                    filetemp.LocalName = DateTime.Now.ToString("yy-mm-dd") + "_" + GenerateCode(4) + "_" + fileName;
                    filetemp.PathFile = extension.ToUpper();
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(file.LocalFileName) + "\\" + extension.ToUpper()))
                    {
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(file.LocalFileName) + "\\" + extension.ToUpper());
                    }
                    string filePath = System.IO.Path.GetDirectoryName(file.LocalFileName)+"\\"+extension.ToUpper()+ "\\" + filetemp.LocalName;
                    System.IO.File.Copy(file.LocalFileName, filePath);
                    System.IO.File.Delete(file.LocalFileName);
                    files.Add(filetemp);
                }
                IEnumerable<File_MT> newFile = fileService.CreateFiles(files);
                response.SetData(newFile);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Update Status
        [HttpPut]
        [Route("")]
        public HttpResponseMessage Put([FromBody]File_MT statusUpdate)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                bool isUpdated = fileService.UpdateFile(statusUpdate);
                response.SetData(isUpdated);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpGet]
        [Route("{FileIDSys}")]
        public HttpResponseMessage Get(string FileIDSys)
        {
            ResponseData<bool> response = new ResponseData<bool>();
            try
            {
                fileService.GetFile(FileIDSys);
                response.SetData(true);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        //Get Data
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ResponseData<IEnumerable<File_MT>> response = new ResponseData<IEnumerable<File_MT>>();
            try
            {
                IEnumerable<File_MT> status = fileService.GetFiles();
                response.SetData(status);
            }
            catch (Validation.ValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        string GenerateCode(int number)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            var chars = Enumerable.Range(0, number)
                            .Select(x => pool[rnd.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

    }
}