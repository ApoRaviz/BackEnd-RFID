﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Common.Utility.UtilityHelpers
{
    public class UtilityHelper
    {        
       
        public static string GetHandleErrorMessageException(ErrorEnum errorEnum, string internalMessage)
        {

            return string.Format("Error #{0}: {1}", errorEnum.GetValue(), internalMessage ?? errorEnum.GetDescription());
        }

        public static IIdentity GetIdentity()
        {
            try
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpContext.Current.GetOwinContext().Authentication.User.Identity;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public static async Task<HttpClient> GetHttpClientForIdentityServerAsync(string baseUrl)
        {
            var identityUrl = baseUrl;
            var nonce = "N" + new Random().Next() + "" + DateTime.Now.ToString("yyyyMMddTHHmmss");
            var state = DateTime.Now.ToString("yyyyMMddTHHmmss") + "" + new Random().Next();
            identityUrl += $"&nonce={nonce}&state={state}";
            
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(identityUrl);
            response.EnsureSuccessStatusCode();
            if (response.RequestMessage.RequestUri.LocalPath.EndsWith("error"))
            {
                throw new Exception("UnAuthorized!");
            }
            var fragmentSplits = response.RequestMessage.RequestUri.Fragment.Split('&');
            var result = new Dictionary<string, string>();
            foreach (string f in fragmentSplits)
            {
                var path = f.Split('=');
                result.Add(path[0], path[1]);
            }
            var accessToken = result["access_token"];
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            return client;
        }

        public static string B2H(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string B2H(string bin)
        {
            StringBuilder result = new StringBuilder(bin.Length / 8 + 1);

            int mod4Len = bin.Length % 8;
            if (mod4Len != 0)
            {
                bin = bin.PadLeft(((bin.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < bin.Length; i += 8)
            {
                string eightBits = bin.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        public static string H2B(string hex)
        {
            List<string> _hex = new List<string>();
            foreach (var h in hex)
            {
                string _h = Convert.ToString(Convert.ToInt32(h.ToString(), 16), 2).PadLeft(4, '0');
                _hex.Add(_h);
            }
            return String.Join(String.Empty, _hex.ToArray());
        }

        public static string B2D(string bin)
        {
            return Convert.ToInt64(bin, 2).ToString();
        }

        public static string D2B(string dec)
        {
            int x = int.Parse(dec);
            var bitConversion = new List<string>();
            while (x >= 0)
            {
                if (x == 0)
                {
                    bitConversion.Add("0");
                    break;
                }
                bitConversion.Add((x % 2).ToString(CultureInfo.InvariantCulture));
                x /= 2;
            }
            bitConversion.Reverse();
            return string.Join("", bitConversion.ToArray());
        }

        public static string D2H(string dec)
        {
            string bin = D2B(dec);
            return B2H(bin);
        }

        public static string H2D(string hex)
        {
            string bin = H2B(hex);
            return B2D(bin);
        }

    }
}
