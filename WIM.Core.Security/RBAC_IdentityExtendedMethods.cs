using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Security;
using WIM.Core.Security.Entity;

public static class RBAC_ExtendedMethods_4_Principal
{
    public static string GetUserId(this IIdentity _identity)
    {
        string _retVal = "";
        try
        {
            if (_identity != null && _identity.IsAuthenticated)
            {
                var ci = _identity as ClaimsIdentity;
                string _userId = ci?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(_userId))
                {
                    _retVal = _userId;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }

    public static string GetUserName(this IIdentity _identity)
    {
        string _retVal = "";
        try
        {
            if (_identity != null && _identity.IsAuthenticated)
            {
                var ci = _identity as ClaimsIdentity;

                if (!string.IsNullOrEmpty(ci.Name))
                {
                    _retVal = ci.Name;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }

    public static int GetProjectIDSys(this IIdentity _identity)
    {
        try
        {
            if (_identity != null && _identity.IsAuthenticated)
            {
                var ci = _identity as ClaimsIdentity;

                var exp = (from c in ci.Claims
                           where c.Type == "ProjectIDSys"
                           select c).SingleOrDefault();

                if (exp != null)
                {
                    return int.Parse(exp.Value);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return 0;
    }

    public static bool HasPermission(this IPrincipal _principal, string _requiredPermission)
    {
        bool _retVal = false;
        try
        {
            if (_principal != null && _principal.Identity != null && _principal.Identity.IsAuthenticated)
            {
                var ci = _principal.Identity as ClaimsIdentity;
                string _userId = ci?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(_userId))
                {
                    ApplicationUser _authenticatedUser = ApplicationUserManager.GetUser(_userId);
                    _retVal = _authenticatedUser.IsPermissionInUserRoles(_requiredPermission);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }


    public static bool HasPermission(this IPrincipal _principal, HttpRequestMessage _request)
    {
        bool _retVal = false;
        try
        {
            var ci = _principal.Identity as ClaimsIdentity;
            string OTPCONFIRM = ci.Claims.Where(c => c.Type == "OTPCONFIRM")
          .Select(c => c.Value).SingleOrDefault();

            string reqUrl = StringHelper.GetRequestUrl(_request.RequestUri.PathAndQuery);

            string fullUrl = _request.Method + reqUrl;
            fullUrl = fullUrl.Replace("wimapi/", "");
            string[] fullUrlplit = fullUrl.Split('?');
            reqUrl = fullUrlplit[0];
            string UpperReqUrl = reqUrl.ToUpper();
           

            if (reqUrl.Last() == '/')
            {
                reqUrl = reqUrl.Substring(0, reqUrl.Length - 1);

            }

            int indexFirstslash = reqUrl.IndexOf('/');
            int indexapiv1 = reqUrl.IndexOf("/api/v1");
            if (indexFirstslash != -1 && indexapiv1 != -1)
            {
                string first = reqUrl.Substring(0, indexFirstslash);
                string second = reqUrl.Substring(indexapiv1);
                //reqUrl = reqUrl.Substring(indexFirstslash, indexapiv1 - indexFirstslash);
                reqUrl = first + second;
            }


            if (OTPCONFIRM != "True")
            {
                return false;
            }

            var claims = (from c in ci.Claims
                          where c.Type == "UrlPermission"
                          select c).ToList();

            // Sub String v1/customers/ => v1/customers  



            //url request
            string[] reqUrlSplit = reqUrl.Split('/');
            string[] upperReqUrlSplit = UpperReqUrl.Split('/');
            if (upperReqUrlSplit.Length >= 4)
            {
                foreach (var c in claims)
                {
                    //url from base
                    string[] permissions = c.Value.Split('/');
                    string permission = permissions[0] + ApiHashTableHelper.apiTable[permissions[1]].ToString().ToUpper();
                    string[] permissUrlSplit = permission.Split('/');

                    string urlVerify = "";
                    if ((permissUrlSplit[0] == upperReqUrlSplit[0] && permissUrlSplit[3] == upperReqUrlSplit[3] && permissUrlSplit.Length == upperReqUrlSplit.Length))
                    //|| (reqUrlSplit[0]=="POST" && permissUrlSplit[3] == reqUrlSplit[3]))
                    {
                        bool isUrlNotEqual = false;

                        for (int i = 0; i < permissUrlSplit.Length; i++)
                        {
                            // permisUrl /api/v1/customers/zzz
                            // reqUrl   /api/v1/customers/abc-123

                            // urlVerify   /api/v1/customers/1

                            //bool isReqUrlNum = int.TryParse(reqUrlSplit[i], out int reqUrlNum);
                            //bool isPermisUrlNum = int.TryParse(reqUrlSplit[i], out int permissUrlNum);

                            if ((permissUrlSplit[i] == upperReqUrlSplit[i])
                                || (permissUrlSplit[i] == "@")
                                || (/*isReqUrlNum && */permissUrlSplit[i] == "1"))
                            {
                                urlVerify += (i == 0 ? "" : "/") + permissUrlSplit[i];
                            }
                            else
                            {
                                isUrlNotEqual = true;
                                break;
                            }
                        }

                        if (isUrlNotEqual)
                        {
                            _retVal = false;
                            //break;
                        }
                    }
                    if (permission == urlVerify)
                    {
                        _retVal = true;
                        break;
                    }
                }
            }
        }
        catch (IndexOutOfRangeException)
        {
            throw;
        }
        catch (Exception ex)
        {
            using (System.IO.StreamWriter _testData = new System.IO.StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/HasPermission.txt"), true))
            {
                _testData.WriteLine(ex.Message); // Write the file.
            }
            throw;
        }

        return _retVal;
    }


    public static bool IsTimeOutToken(this IPrincipal _principal)
    {

        bool _retVal = false;
        try
        {
            var ci = _principal.Identity as ClaimsIdentity;


            var exp = (from c in ci.Claims
                       where c.Type == "exp"
                       select c).ToList();
            if (exp.Count > 0)
            {
                _retVal = true;
            }
        }
        catch (Exception ex)
        {
            using (System.IO.StreamWriter _testData = new System.IO.StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/HasPermission.txt"), true))
            {
                _testData.WriteLine(ex.Message); // Write the file.
            }
            throw;
        }
        return _retVal;
    }

    public static bool IsSysAdmin(this IPrincipal _principal)
    {
        bool _retVal = false;
        try
        {
            if (_principal != null && _principal.Identity != null && _principal.Identity.IsAuthenticated)
            {
                var ci = _principal.Identity as ClaimsIdentity;
                string _userId = ci?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(_userId))
                {
                    ApplicationUser _authenticatedUser = ApplicationUserManager.GetUser(_userId);
                    //_retVal = _authenticatedUser.IsSysAdmin();
                    _retVal = _authenticatedUser.IsSysAdmin;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }

    public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
    {
        string _retVal = string.Empty;
        try
        {
            if (identity != null)
            {
                var claim = identity.FindFirst(claimType);
                _retVal = claim?.Value;
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }

    public static bool IsUrlIgnored(this IPrincipal _principal, HttpRequestMessage _request)
    {
        string[] urlIgnore = {
                    "GET/api/v1/account/users/mobile/otp",
                    "POST/api/v1/account/users/mobile/otp",
                    "POST/api/v1/account/renewtoken",
                    "GET/api/v1/MenuProjectMappings/parent/1",
                    "GET/api/v1/Users/customers",
                    "GET/api/v1/customers/projects",
                    "POST/api/v1/account/assignProject",
                    "GET/api/v1/Persons",
                    "GET/api/v1/helpers/tableColumnsDescription",
                    "GET/api/v1/Projects/select",
                    "POST/api/v1/account/Logout",
                    "POST/api/v1/Account/ChangePassword",
                    "GET/api/v1/Roles/user",
                    //Menu Side Url
                    "GET/api/v1/MenuProjectMappings/menu/",
                    //Url Ignore ChkOTP
                    "POST/api/v1/account/assignProject"
                };
        string reqUrlnew = _request.Method + StringHelper.GetRequestUrl(_request.RequestUri.PathAndQuery);
        if (_request.RequestUri.PathAndQuery.Last() == '/')
        {
            reqUrlnew = _request.RequestUri.PathAndQuery.Substring(0, _request.RequestUri.PathAndQuery.Length - 1);

        }
        string project = "GET/api/v1/Projects";
        string menuSideUrl = "GET/api/v1/MenuProjectMappings/menu/";
        string urlIgnoreChkOTP = "POST/api/v1/account/assignProject";
        var ci = _principal.Identity as ClaimsIdentity;
        string OTPCONFIRM = ci.Claims.Where(c => c.Type == "OTPCONFIRM")
          .Select(c => c.Value).SingleOrDefault();
        return urlIgnore.Contains(reqUrlnew) || reqUrlnew.Contains(menuSideUrl) || reqUrlnew.Contains(project) || (OTPCONFIRM == "True" && urlIgnoreChkOTP.Contains(reqUrlnew));
    }


}