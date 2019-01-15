using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http.ModelBinding;
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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return client;

        }

        public static async Task<T> GetAsync<T>(string url, string identityUrl = null)
        {
            HttpClient client = new HttpClient();
            if (!string.IsNullOrEmpty(identityUrl))
            {
                client = await GetHttpClientForIdentityServerAsync(identityUrl);
            }
            HttpResponseMessage response = await client.GetAsync(url);
            return await MapHttpClientResponseDataAsync<T>(response);
        }

        public static async Task<T> PostAsync<T>(string url, StringContent content, string identityUrl = null)
        {
            HttpClient client = new HttpClient();
            if (!string.IsNullOrEmpty(identityUrl))
            {
                client = await GetHttpClientForIdentityServerAsync(identityUrl);
            }
            HttpResponseMessage response = await client.PostAsync(url, content);
            return await MapHttpClientResponseDataAsync<T>(response);
        }

        private static async Task<T> MapHttpClientResponseDataAsync<T>(HttpResponseMessage response)
        {
            string body = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<APIResponse>(body);

            if (responseData.StatusCode != 200)
            {
                string message = responseData.Message;
                if (responseData.ResponseException != null)
                {
                    message += $"\n{responseData.ResponseException.ExceptionMessage}";
                }
                throw new Exception(message);
            }
            var result = JsonConvert.SerializeObject(responseData.Result);
            return JsonConvert.DeserializeObject<T>(result);
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

    public class APIResponse
    {
        public APIResponse(int statusCode, string message = "", object result = null, ApiError responseException = null, string version = "1.0.0.0")
        {
            StatusCode = statusCode;
            Message = message;
            Result = result;
            ResponseException = responseException;
            Version = version;
        }

        public string Version { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public ApiError ResponseException { get; set; }
        public object Result { get; set; }
    }

    public class ApiError
    {
        public ApiError(bool isError, string exceptionMessage, string details, string referenceErrorCode, string referenceDocumentLink, IEnumerable<ValidationError> validationErrors)
        {
            IsError = isError;
            ExceptionMessage = exceptionMessage;
            Details = details;
            ReferenceErrorCode = referenceErrorCode;
            ReferenceDocumentLink = referenceDocumentLink;
            ValidationErrors = validationErrors;
        }

        public bool IsError { get; set; }
        public string ExceptionMessage { get; set; }
        public string Details { get; set; }
        public string ReferenceErrorCode { get; set; }
        public string ReferenceDocumentLink { get; set; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }
    }
}
