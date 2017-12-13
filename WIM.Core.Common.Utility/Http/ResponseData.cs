using WIM.Core.Common.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Extensions;
using System.Net.Http;
using WIM.Core.Common.Utility.Helpers;
using System.Data.Entity.Infrastructure;

namespace WIM.Core.Common.Utility.Http
{
    public class ResponseData<DataType> : IResponseData<DataType>
    {
        public HttpStatusCode Status { get; set; }
        public IList<ValidationError> Errors { get; set; }
        public DataType Data { get; set; }

        public ResponseData()
        {
            this.Status = HttpStatusCode.OK;
            this.Errors = new List<ValidationError>();
        }
        public void SetStatus(HttpStatusCode httpStatusCode)
        {
            this.Status = httpStatusCode;
        }

        public void SetErrors(IList<ValidationError> errors)
        {
            errors.Each(error =>
            {
                this.Errors.Add(error); });
        }

        public void SetErrors(DbUpdateException error)
        {
            ValidationError x = new ValidationError("500", error.Message);
            this.Errors.Add(x);
        }

        public void SetData(DataType data)
        {
            this.Data = data;
        }

        public HttpStatusCode GetStatus()
        {
            return this.Status;
        }

        public IList<ValidationError> GetErrors()
        {
            return this.Errors;
        }

        public DataType GetData()
        {
            return this.Data;
        }

    }
}
