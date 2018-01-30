using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Utility.Helpers;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;

namespace WIM.WebApi.Auth
{
    public class IdentityAuthAttribute : AuthorizationFilterAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {

            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            //if (!principal.Identity.IsAuthenticated)
            //{
            //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            //    return Task.FromResult<object>(null);
            //}

            //string url = actionContext.Request.RequestUri.PathAndQuery;
            //string method = actionContext.Request.Method.ToString();    

            if (principal.IsUrlIgnored(actionContext.Request))
            {
                return Task.FromResult<object>(null);
            }

            //if (!principal.HasPermission(method + url))
            //#JobComment
            if (!principal.IsTimeOutToken())
            {
                //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E401));
                if (actionContext.Request.Method.Equals(HttpMethod.Post).Equals(HttpMethod.Put))
                    ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E402));
                IResponseData<int> response = new ResponseData<int>();
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.Unauthorized);
                actionContext.Response = actionContext.Request.CreateResponse<IResponseData<int>>(HttpStatusCode.Unauthorized, response);
                return Task.FromResult<object>(null);
            }

            //if (!principal.HasPermission(actionContext.Request) && !principal.IsSysAdmin())
            //{
            //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
            //    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E403));
            //    IResponseData<int> response = new ResponseData<int>();
            //    response.SetErrors(ex.Errors);
            //    response.SetStatus(HttpStatusCode.Unauthorized);
            //    actionContext.Response = actionContext.Request.CreateResponse<IResponseData<int>>(HttpStatusCode.Forbidden, response);
            //    return Task.FromResult<object>(null);
            //}

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);

        }
    }
}