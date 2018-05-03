using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using WIM.Core.Entity.Common;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Service;
using WIM.Core.Service.Impl;

namespace WMS.WebApi.Controller
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

                    // จ๊อบ Comment กำลัง Dev อยู่
                    string urlFrontEnd;
                    if (request.Method == HttpMethod.Get)
                    {
                        urlFrontEnd = request.GetQueryNameValuePairs().FirstOrDefault(kv => kv.Key == "urlFrontEnd").Value;
                    }
                    else
                    {
                        JObject body = JsonConvert.DeserializeObject<JObject>(userLogEntry.RequestContentBody);
                        urlFrontEnd = body["urlFrontEnd"] + "";
                    }

                    if (!string.IsNullOrEmpty(urlFrontEnd) && urlFrontEnd[0] == '/')
                    {
                        urlFrontEnd = urlFrontEnd.Substring(1);
                    }
                    urlFrontEnd = urlFrontEnd.Split('?')[0];
                    urlFrontEnd = urlFrontEnd.Split(';')[0];

                    IMenuService menuService = new MenuService();
                    Menu_MT menu = menuService.GetMenuByUrl(urlFrontEnd);

                    userLogEntry.RequestUriFrondEnd = menu.Url;
                    userLogEntry.RequestMenuNameFrontEnd = menu.MenuName;

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