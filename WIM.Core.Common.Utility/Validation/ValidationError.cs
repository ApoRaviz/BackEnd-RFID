using System.Collections.Generic;
using System.Net;

namespace WIM.Core.Common.Utility.Validation
{
    public class ValidationError : IResourceItem
    {
        public ValidationError(string key, string message, IList<string> args = null)
        {
            this.Key = key;
            this.Message = message;
            this.Params = args ?? new List<string>();

        }

        public ValidationError(string key, IList<string> args = null)
        {
            this.Key = key;
            this.Params = args ?? new List<string>();
        }


        public ValidationError(string key, string message, HttpStatusCode httpcode, string action = null)
        {
            this.Key = key;
            this.Message = message;
            this.HttpCode = httpcode;
            this.Action = action;

        }

        public ValidationError(ErrorEnum errorEnum)
        {
            this.Key = errorEnum.GetValue();
            this.Message = errorEnum.GetDescription();
            this.HttpCode = errorEnum.GetHttpCode();
            this.Action = null;
        }

        public ValidationError(ErrorEnum errorEnum, string message)
        {
            this.Key = errorEnum.GetValue();
            this.Message = errorEnum.GetDescription();
            this.HttpCode = errorEnum.GetHttpCode();
            this.Action = null;
        }

        public string Key { get; set; }
        public string Message { get; set; }
        public HttpStatusCode HttpCode { get; set; }
        public IList<string> Params { get; set; }
        public string Action { get; set; }

    }
}
