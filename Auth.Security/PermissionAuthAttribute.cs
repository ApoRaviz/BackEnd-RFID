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
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;

namespace Auth.Security
{
    public class PermissionAuthAttribute : AuthorizationFilterAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (principal.IsUrlIgnored(actionContext.Request))
            {
                return Task.FromResult<object>(null);
            }

            if (!principal.HasPermission(actionContext.Request) && !principal.IsSysAdmin())
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
                AppValidationException ex = new AppValidationException(ErrorEnum.NO_PERMISSION);
                IResponseData<int> response = new ResponseData<int>();
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.Unauthorized);
                actionContext.Response = actionContext.Request.CreateResponse<IResponseData<int>>(HttpStatusCode.Forbidden, response);
            }

            return Task.FromResult<object>(null);

        }
    }
}
