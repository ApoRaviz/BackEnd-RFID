using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using WIM.Core.Security.Entity;

namespace WIM.Core.Security
{
    public static class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ApplicationUser user,string roleID = null)
        {
            List<Claim> claims = new List<Claim>();

            if (!user.Roles.Any() || string.IsNullOrEmpty(roleID))
            {
                return claims;
            }
            
            

            //foreach (var r in user.Roles)
            //    {
                    
                    ApplicationRole role = ApplicationRoleManager.GetRolePermissions(roleID);
                    if (role == null)
                    {
                        return claims;
                    }
                    foreach (var perm in role.Permissions)
                    {
                        if (perm.MenuIDSys == null || perm.ProjectIDSys == null)
                        {
                            continue;
                        }
                //string url = perm.Method + perm.MenuProjectMapping.Menu_MT.Url;
                string url = perm.Method + "/" + perm.Api_MT.ApiIDSys;

                if (claims.Any(x => x.Type == "UrlPermission" && x.Value == url))
                {
                    continue;
                }
                claims.Add(CreateClaim("UrlPermission", url));
            }
                //}
                                
            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }        

    }
}