﻿using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using WIM.Core.Common.Helpers;
using WIM.Core.Common.Utility.UtilityHelpers;
using Auth.Security;
using Auth.Security.Entity;
using WIM.Core.Common.Utility.Validation;

public static class RBAC_ExtendedMethods_4_Principal
{
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

    public static bool IsOtpConfirmed(this IPrincipal _principal)
    {
        var ci = _principal.Identity as ClaimsIdentity;

        string OTPCONFIRM = ci.Claims.Where(c => c.Type == "OTPCONFIRM")
      .Select(c => c.Value).SingleOrDefault();

        return OTPCONFIRM == "True";
    }


    public static bool HasPermission(this IPrincipal _principal, HttpRequestMessage _request)
    {
        bool _retVal = false;
        try
        {
            var ci = _principal.Identity as ClaimsIdentity;          
            string reqUrl = StringHelper.GetRequestUrl(_request.Method +_request.RequestUri.PathAndQuery);
            string[] upperReqUrlSplit = reqUrl.ToUpper().Split('/');
            if (upperReqUrlSplit.Length > 3)
            {

                var claims = (from c in ci.Claims
                              where c.Type == "UrlPermission"
                              select c).ToList();

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
            throw new AppValidationException(ErrorEnum.NO_PERMISSION);
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
                    "GET/api/v1/external/programVersion/Fuji",
                    // Menu Side Url
                    "GET/api/v1/MenuProjectMappings/menu/",
                    // Url Ignore ChkOTP
                    "POST/api/v1/account/assignProject",
                    // Demo
                    "GET/api/v1/demo/func7",
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