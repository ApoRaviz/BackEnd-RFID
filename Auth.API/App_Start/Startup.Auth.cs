using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Auth.Security.Context;
using Auth.Security;
using Auth.Security.Providers;

namespace Auth.API
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
           
            app.CreatePerOwinContext(SecurityDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
              
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/v1/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt16(ConfigurationManager.AppSettings["as:ExToken"])),
               
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat(ConfigurationManager.AppSettings["as:baseUrl"])
             
            };

            app.UseOAuthAuthorizationServer(OAuthOptions);
        }

        public void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = ConfigurationManager.AppSettings["as:baseUrl"];
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }
    }
}