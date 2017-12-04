using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Extensions;

namespace WIM.Core.Common.Utility.Http
{      
    public class AcceptJsonHttpMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string value = request.GetHeaderValue("Accept");
            
            if (request.Method.ToString() != "OPTIONS" && value != "application/json" && !UrlExceptAcceptJson.UrlList.Contains(request.RequestUri.LocalPath))
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent("Not Acceptable! T_T")
                };
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }           
            return base.SendAsync(request, cancellationToken);
        }
    }
    public static class UrlExceptAcceptJson
    {
        public static List<string> UrlList
        {
            get
            {
                return new List<string>()
                {
                    "/api/v1/account/ConfirmEmail"
                };
            }
        }
    }
}
