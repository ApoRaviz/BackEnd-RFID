using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Master.WebApi.Controllers
{
    [RoutePrefix("api/v1/demo")]
    public class DemoController : ApiController
    {

        public DemoController()
        {

        }

        [HttpGet]
        [Route("func1")]
        public void Func1()
        {
           
        }

        [HttpGet]
        [Route("func2")]
        public void Func2()
        {

        }
    }

    public class Email
    {
        public string Title { get; set; }
        public Email()
        {
            Title = "Default";
        }
    }
}