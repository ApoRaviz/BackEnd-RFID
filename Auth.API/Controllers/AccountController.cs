using Auth.API.Providers.Firebase;
using Auth.API.Providers.Firebase.Model;
using Auth.API.Result;
using Auth.Security;
using Auth.Security.Entity;
using Auth.Security.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.Person;
using WIM.Core.Entity.UserManagement;
using WIM.Core.Service.Impl;

namespace Auth.API.Controllers
{
    [RoutePrefix("api/v1/account")]
    public class AccountController : BaseApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private int ExToken = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["as:ExToken"]);

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = Microsoft.AspNet.Identity.IdentityExtensions.GetUserName(User.Identity),
                HasRegistered = externalLogin == null,
                IsAdmin = User.IsSysAdmin(),
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        //[Authorize(Roles = "Admin")]
        //[ClaimsAuthorization(ClaimType = "Phone", ClaimValue = "123456789")]        
        [Route("users/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Route("assignCustomer")]
        [HttpPost]
        public IHttpActionResult AssignCustomerClaimToUser([FromBody]AssignCustomerClaimBinding customerClaimBinding)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = User.Identity as ClaimsIdentity;
            foreach (var claim in identity.Claims)
            {
                UserManager.RemoveClaim(User.Identity.GetUserId(), claim);
            }
            UserManager.AddClaim(User.Identity.GetUserId(), new Claim("CustIDSys", customerClaimBinding.CustIDSys));
            return Ok();
        }

        [Route("assignOTP")]
        [HttpPost]
        public IHttpActionResult AssignOTPClaimToUser([FromBody]AssignCustomerClaimBinding customerClaimBinding)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = User.Identity as ClaimsIdentity;
            foreach (var claim in identity.Claims)
            {
                UserManager.RemoveClaim(User.Identity.GetUserId(), claim);
            }
            UserManager.AddClaim(User.Identity.GetUserId(), new Claim("OTP", customerClaimBinding.CustIDSys));
            return Ok();
        }

        //[HttpPost]
        //[Route("login")]
        //public HttpResponseMessage Login()
        //{
        //    ApplicationUser appUser = ApplicationUserManager.GetUserByUserNamePassword("13007", "Zxcv123!");
        //    this.GetOTP(appUser.Id);
        //    return null;
        //}

        [HttpGet]
        [Route("users/mobile/otp")]
        public HttpResponseMessage GetOTP()
        {
            IResponseData<string> response = new ResponseData<string>();
            try
            {
                Firebase fireb = new Firebase();
                FirebaseModelSand fireBaseParam = new FirebaseModelSand();
                string token;
                UserService users = new UserService();
                Random rnd = new Random();
                int key;
                key = rnd.Next(100000, 999999);
                token = users.GetFirebaseTokenMobileByUserID(User.Identity.GetUserId(), key);
                fireBaseParam.to = token;
                fireBaseParam.notification.title = "OTP";
                fireBaseParam.notification.body = key.ToString();
                IRestResponse res = fireb.SendNotificationsToMobile(fireBaseParam);
                response.SetData("OTP");
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }

        [HttpPost]
        [Route("users/mobile/otp")]
        public async Task<HttpResponseMessage> CheckOTPAsync([FromBody]AssignOTPClaimBinding OTPClaimBinding)
        {
            IResponseData<Dictionary<string, string>> response = new ResponseData<Dictionary<string, string>>();
            try
            {
                if (!ModelState.IsValid)
                {
                    return null;
                }

                Dictionary<string, string> Json = new Dictionary<string, string>();
                User users = new UserService().GetUserByUserID(User.Identity.GetUserId());



                if (OTPClaimBinding.OTP.Equals(users.KeyOTP) && DateTime.Now.AddMinutes(-2) < users.KeyOTPDate)
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(users.UserID);
                    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager, "JWT");
                    foreach (var claim in oAuthIdentity.Claims)
                    {
                        UserManager.RemoveClaim(User.Identity.GetUserId(), claim);
                    }
                    oAuthIdentity.AddClaim(new Claim("OTPCONFIRM", "True"));
                    oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
                    oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));
                    oAuthIdentity.AddClaim(new Claim("OTP", OTPClaimBinding.OTP.ToString()));
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IssuedUtc = DateTime.Now;
                    props.ExpiresUtc = DateTime.Now.AddMinutes(this.ExToken);
                    var ticket = new AuthenticationTicket(oAuthIdentity, props);
                    CustomJwtFormat auth = new CustomJwtFormat(System.Configuration.ConfigurationManager.AppSettings["as:baseUrl"]);
                    string token = auth.Protect(ticket);
                    TimeSpan spEx = TimeSpan.FromMinutes(ExToken);
                    Json.Add("access_token", token);
                    Json.Add("expires_in", Convert.ToInt32(spEx.TotalSeconds).ToString());
                    Json.Add("status", "200");
                }
                else if (!OTPClaimBinding.OTP.Equals(users.KeyOTP))
                {

                    Json.Add("message", "OTP Invalid");
                    Json.Add("status", "4012");
                }
                else if (DateTime.Now.AddMinutes(-2) > users.KeyOTPDate)
                {

                    Json.Add("message", "OTP Expires");
                    Json.Add("status", "4011");
                }


                response.SetData(Json);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [Authorize]
        [HttpPost]
        [Route("renewtoken")]
        public async Task<HttpResponseMessage> ReTokenAsy([FromBody]ParamReToken param)
        {
            IResponseData<Dictionary<string, string>> response = new ResponseData<Dictionary<string, string>>();
            Dictionary<string, string> Json = new Dictionary<string, string>();
            try
            {
                string roleID = "";
                
                Boolean IsSysAdmin = User.IsSysAdmin();
                if (!IsSysAdmin)
                {
                    roleID = new RoleService().GetRoleByUserAndProject(User.Identity.GetUserId(), User.Identity.GetProjectIDSys());
                    if (!ModelState.IsValid && string.IsNullOrEmpty(roleID))
                    {
                        return null;
                    }
                }

                if (!string.IsNullOrEmpty(param.Time))
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager, "JWT");
                    foreach (var claim in oAuthIdentity.Claims)
                    {
                        UserManager.RemoveClaim(User.Identity.GetUserId(), claim);
                    }
                    oAuthIdentity.AddClaim(new Claim("ProjectIDSys", User.Identity.GetProjectIDSys().ToString()));
                    if (!IsSysAdmin)
                    {
                        oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user, roleID));
                    }
                    oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));
                    oAuthIdentity.AddClaim(new Claim("OTPCONFIRM", "True"));
                    AuthenticationProperties props = new AuthenticationProperties();
                    DateTime dateLatest = Convert.ToDateTime(param.Time, CultureInfo.CreateSpecificCulture("en-US"));
                    DateTime dateCur = DateTime.Now;
                    double timeAdd = ExToken - (dateCur - dateLatest).TotalMinutes;
                    timeAdd = (timeAdd > ExToken) ? ExToken : timeAdd < 1 ? ExToken : timeAdd;
                    TimeSpan spEx = TimeSpan.FromMinutes(timeAdd);
                    props.IssuedUtc = DateTime.Now;
                    props.ExpiresUtc = DateTime.Now.AddMinutes(timeAdd);
                    var ticket = new AuthenticationTicket(oAuthIdentity, props);
                    CustomJwtFormat auth = new CustomJwtFormat(System.Configuration.ConfigurationManager.AppSettings["as:baseUrl"]);
                    string token = auth.Protect(ticket);
                    Json.Add("access_token", token);
                    Json.Add("expires_in", Convert.ToInt32(spEx.TotalSeconds).ToString());
                    Json.Add("status", "200");
                }
                else
                {
                    Json.Add("status", "401");
                }

                response.SetData(Json);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);
        }


        [Route("assignProject")]
        [HttpPost]
        public async Task<HttpResponseMessage> AssignProjectClaimToUserAsync([FromBody]AssignProjectClaimBinding projectClaimBinding)
        {
            IResponseData<Dictionary<string, string>> response = new ResponseData<Dictionary<string, string>>();
            try
            {
                string roleID = "";
                if (!User.IsSysAdmin())
                {
                    roleID = new RoleService().GetRoleByUserAndProject(User.Identity.GetUserId(), projectClaimBinding.ProjectIDSys);
                    if (!ModelState.IsValid && string.IsNullOrEmpty(roleID))
                    {
                        return null;
                    }
                }
                //object CanAccessProject = ApplicationUserManager.GetUserAccessProject(User.Identity.GetUserId());
                Dictionary<string, string> Json = new Dictionary<string, string>();
                ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager, "JWT");
                //foreach (var claim in oAuthIdentity.Claims)
                //{
                //    UserManager.RemoveClaim(User.Identity.GetUserId(), claim);
                //}
                oAuthIdentity.AddClaim(new Claim("OTPCONFIRM", "True"));
                oAuthIdentity.AddClaim(new Claim("ProjectIDSys", projectClaimBinding.ProjectIDSys.ToString()));
                if (User.IsSysAdmin())
                { oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user)); }
                else
                { oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user, roleID)); }
                oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));
                AuthenticationProperties props = new AuthenticationProperties();
                props.IssuedUtc = DateTime.Now;
                props.ExpiresUtc = DateTime.Now.AddMinutes(this.ExToken);
                var ticket = new AuthenticationTicket(oAuthIdentity, props);
                CustomJwtFormat auth = new CustomJwtFormat(System.Configuration.ConfigurationManager.AppSettings["as:baseUrl"]);
                string token = auth.Protect(ticket);
                TimeSpan spEx = TimeSpan.FromMinutes(ExToken);
                Json.Add("access_token", token);
                Json.Add("expires_in", Convert.ToInt32(spEx.TotalSeconds).ToString());
                Json.Add("status", "200");
                Person_MT Persons = new PersonService().GetPersonByPersonIDSys(User.Identity.GetUserId());
                Json.Add("name", Persons.Name);

                var Project = new ProjectService().GetProjectByProjectIDSysIncludeModule(projectClaimBinding.ProjectIDSys);
                if (Project != null)
                    Json.Add("project", Newtonsoft.Json.JsonConvert.SerializeObject(Project));

                response.SetData(Json);
            }
            catch (AppValidationException ex)
            {
                response.SetErrors(ex.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
            }
            return Request.ReturnHttpResponseMessage(response);



            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var identity = User.Identity as ClaimsIdentity;
            //foreach (var claim in identity.Claims)
            //{
            //    UserManager.RemoveClaim(User.Identity.GetUserId(), claim);
            //}
            //UserManager.AddClaim(User.Identity.GetUserId(), new Claim("ProjectIDSys", projectClaimBinding.ProjectIDSys));
            //return Ok();
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut("JWT");
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (ApplicationUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordBindingModel model)
        {
            ResponseData<IdentityResult> response = new ResponseData<IdentityResult>();
            IdentityResult result = new IdentityResult();

            result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                response.SetErrors(result.Errors);
                response.SetStatus(HttpStatusCode.PreconditionFailed);
                return Request.ReturnHttpResponseMessage(response);
            }
            response.SetData(result);
            return Request.ReturnHttpResponseMessage(response);
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        //# Oil Comment
        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        /*[AllowAnonymous]
        [Route("email")]
        public async Task<IHttpActionResult> SendEmail()
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }
            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
            //var callbackUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["as:baseUrl"] + string.Format("/api/v1/account/ConfirmEmail?userId={0}&code={1}", user.Id, code));
            await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account", callbackUrl.ToString());

            
            return Ok();
        }*/






        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {

                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }
        #endregion
    }

    public class AssignOTPClaimBinding
    {
        public int OTP { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ParamReToken
    {
        public string Time { get; set; }
        //public string Token { get; set; }
    }


    public class AssignCustomerClaimBinding
    {
        public string CustIDSys { get; set; }
    }

    public class AssignProjectClaimBinding
    {
        public int ProjectIDSys { get; set; }
        public string OTP { get; set; }
    }
}
