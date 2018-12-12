﻿using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace Auth.Security.Providers
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
    
        private readonly string _issuer = string.Empty;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];

            string audienceSecret = ConfigurationManager.AppSettings["as:AudienceSecret"];

            var securityKey = new Microsoft.IdentityModel.Tokens.
        SymmetricSecurityKey(Encoding.UTF8.GetBytes(audienceSecret));

            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, "HS256");

            var issued = data.Properties.IssuedUtc;

            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}