using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using WIM.Core.Common;

namespace WMS.WebApi.Controllers
{
    public class ApiLogHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var userLogEntry = CreateUserLogEntryWithRequestData(request);
            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        userLogEntry.RequestContentBody = task.Result;
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;

                    // Update the API log entry with response info
                    userLogEntry.ResponseStatusCode = response.StatusCode.ToString();
                    userLogEntry.ResponseTimestamp = DateTime.Now;

                    if (response.Content != null)
                    {
                        userLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                        userLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                    }

                    // TODO: Save the API log entry to the database
                    userLogEntry.RequestContentBody = HttpUtility.UrlDecode(userLogEntry.RequestContentBody);
                    new CommonService().WriteUserLog(userLogEntry);

                    return response;
                }, cancellationToken);
        }

        private UserLog CreateUserLogEntryWithRequestData(HttpRequestMessage request)
        {
            var context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
            var routeData = request.GetRouteData();
            var User = context.User.Identity.Name;

            return new UserLog
            {
                //Machine = Environment.MachineName,
                Machine = User,
                RequestContentType = context.Request.ContentType,
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
        }

        private string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented);
        }

        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }    
}