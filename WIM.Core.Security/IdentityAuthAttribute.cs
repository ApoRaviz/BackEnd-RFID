﻿using System;
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
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Security
{
    public class IdentityAuthAttribute : AuthorizationFilterAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {

            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal; 

            if (principal.IsUrlIgnored(actionContext.Request))
            {
                return Task.FromResult<object>(null);
            }
            
            if (!principal.IsTimeOutToken())
            {
                ValidationException ex = new ValidationException(ErrorEnum.E401);

                if (new List<HttpMethod> {
                    HttpMethod.Post,
                    HttpMethod.Put
                }.Contains(actionContext.Request.Method))
                {                    
                    ex = new ValidationException(ErrorEnum.E402);
                }
                    
                IResponseData<int> response = new ResponseData<int>();
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.Unauthorized);
                actionContext.Response = actionContext.Request.CreateResponse<IResponseData<int>>(HttpStatusCode.Unauthorized, response);
                return Task.FromResult<object>(null);
            }

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);

        }
    }
}