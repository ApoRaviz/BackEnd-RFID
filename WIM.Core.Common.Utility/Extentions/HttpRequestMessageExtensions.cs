using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using System.Net.Http;
using WIM.Core.Common.Utility.Http;
using System.Linq;

namespace WIM.Core.Common.Utility.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpResponseMessage ReturnHttpResponseMessage<T>(this HttpRequestMessage request, IResponseData<T> response)
        {
            return request.CreateResponse(response.GetStatus(), response);
        }

        public static string GetHeaderValue(this HttpRequestMessage request, string name)
        {
            IEnumerable<string> values;
            var found = request.Headers.TryGetValues(name, out values);
            if (found)
            {
                return values.FirstOrDefault();
            }

            return null;
        }
    }
}
