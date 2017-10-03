using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Fuji.WebApi.Auth
{
    public static class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ApplicationUser user)
        {
          
            List<Claim> claims = new List<Claim>();

            var daysInWork =  (DateTime.Now.Date - user.LastLogin).TotalDays;

            if (daysInWork > 90)
            {
                claims.Add(CreateClaim("FTE", "1"));
               
            }
            else {
                claims.Add(CreateClaim("FTE", "0"));
            }

            if (!user.Roles.Any())
            {
                return claims;
            }

            foreach (var r in user.Roles)
                {
                    ApplicationRole role = ApplicationRoleManager.GetRolePermissions(r.RoleId);
                    if (role == null)
                    {
                        continue;
                    }
                    foreach (var perm in role.Permissions)
                    {
                        if (perm.MenuIDSys == null || perm.ProjectIDSys == null)
                        {
                            continue;
                        }
                        string url = perm.Method + perm.MenuProjectMapping.Menu.Url;

                        if (claims.Any(x => x.Type == "UrlPermission" && x.Value == url))
                        {
                            continue;
                        }
                        claims.Add(CreateClaim("UrlPermission", url));
                    }
                }
                                
            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }        

    }
}