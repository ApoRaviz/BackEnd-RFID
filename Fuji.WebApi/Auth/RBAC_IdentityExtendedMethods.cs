using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using Fuji.WebApi.Auth;
using Fuji.WebApi.Models;

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
                string _userId = ci != null ? ci.FindFirstValue(ClaimTypes.NameIdentifier) : null;

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

    public static bool HasPermission(this IPrincipal _principal, string _requiredPermission)
    {
        bool _retVal = false;
        try
        {
            if (_principal != null && _principal.Identity != null && _principal.Identity.IsAuthenticated)
            {
                var ci = _principal.Identity as ClaimsIdentity;
                string _userId = ci != null ? ci.FindFirstValue(ClaimTypes.NameIdentifier) : null;

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

    //public static bool HasPermission(this IPrincipal _principal, string _methodUrl)
    public static bool HasPermission(this IPrincipal _principal, HttpRequestMessage _request)
    {

        bool _retVal = false;
        try
        {
            var ci = _principal.Identity as ClaimsIdentity;
            var claims = (from c in ci.Claims
                          where c.Type == "UrlPermission"
                          select c).ToList();

            
            foreach (var c in claims)
            {               
                
                string fullUrl = _request.Method + _request.RequestUri.PathAndQuery;
                fullUrl = fullUrl.Replace("wimapi/", "");
                string[] fullUrlplit = fullUrl.Split('?');
                string reqUrl = fullUrlplit[0];

                // Sub String v1/customers/ => v1/customers  
                if (reqUrl.Last() == '/')
                {
                    reqUrl = reqUrl.Substring(0, reqUrl.Length - 1);
                }    
                
                //url request
                string[] reqUrlSplit = reqUrl.Split('/');

                //url from base
                string[] permissUrlSplit = c.Value.Split('/');

                string urlVerify = "";
                if (permissUrlSplit.Length == reqUrlSplit.Length)
                {
                    bool isUrlNotEqual = false;
                    for (int i = 0; i < permissUrlSplit.Length; i++)
                    {
                        // permisUrl /api/v1/customers/zzz
                        // reqUrl   /api/v1/customers/abc-123

                        // urlVerify   /api/v1/customers/1

                        int reqUrlNum = 0;
                        int permissUrlNum = 0;
                        bool isReqUrlNum = int.TryParse(reqUrlSplit[i], out reqUrlNum);
                        bool isPermisUrlNum = int.TryParse(reqUrlSplit[i], out permissUrlNum);

                        if ((isReqUrlNum == isPermisUrlNum
                            && permissUrlSplit[i] == reqUrlSplit[i]) || (permissUrlSplit[i] == "@parameter_string"))
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
                        break;
                    }
                }
                // /api/v1/customers/zzz == /api/v1/customers/zzz
                if (c.Value == urlVerify)
                {
                    _retVal = true;
                    break;
                }
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
                string _userId = ci != null ? ci.FindFirstValue(ClaimTypes.NameIdentifier) : null;

                if (!string.IsNullOrEmpty(_userId))
                {
                    ApplicationUser _authenticatedUser = ApplicationUserManager.GetUser(_userId);
                    _retVal = _authenticatedUser.IsSysAdmin();                    
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
                _retVal = claim != null ? claim.Value : null;
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }
}