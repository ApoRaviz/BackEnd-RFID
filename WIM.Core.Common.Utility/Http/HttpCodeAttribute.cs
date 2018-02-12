using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.Http
{
    public class HttpCodeAttribute : Attribute
    {
        public HttpCodeAttribute(HttpStatusCode httpcode)
        {
            this.HttpCode = httpcode;
        }

        public HttpStatusCode HttpCode { get; set; }
    }
}
