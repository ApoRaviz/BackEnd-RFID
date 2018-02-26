using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Common.Utility.Http
{
    public interface IResponseData<DataType>
    {
        void SetStatus(HttpStatusCode httpStatusCode);
        void SetErrors(IList<ValidationError> errors);
        void SetErrors(DbUpdateException error);
        void SetData(DataType data);
        HttpStatusCode GetStatus();
        IList<ValidationError> GetErrors();
        DataType GetData();
    }
}
