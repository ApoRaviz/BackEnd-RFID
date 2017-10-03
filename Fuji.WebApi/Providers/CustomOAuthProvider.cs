using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Configuration;
using System.Web.Http.Routing;
using WIM.Core.Security.Entity;
using WIM.Core.Security;

namespace Fuji.WebApi.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            //if(ApplicationUserManager.PasswordHistoryOver3Month(user.Id))
            //{
            //    context.SetError("password_expire", "Your current password is over 3 month,Please change your password.");
            //    return;
            //}
            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
            oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
            oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));
           
            var ticket = new AuthenticationTicket(oAuthIdentity, null);
            
            context.Validated(ticket);
            if (!user.EmailConfirmed)
            {                        
                string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                string codeUrlEncode = HttpUtility.UrlEncode(code);
                var uri = new Uri(ConfigurationManager.AppSettings["as:baseUrl"] + string.Format("/api/v1/account/ConfirmEmail?userId={0}&code={1}", user.Id, codeUrlEncode));
                await userManager.SendEmailAsync(user.Id,"Confirm your account", uri.ToString());
            }
            ApplicationUserManager.PasswordHistoryOverYear(user.Id);
        }
    }
}