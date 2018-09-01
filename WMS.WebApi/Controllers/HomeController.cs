using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using WIM.Core.Common.Utility.Validation;
using WMS.Entity.ImportManagement;
using WMS.Service;
using WMS.WebApi.ServiceBus.IntegrationEvents;

namespace WMS.WebApi.Controller
{
    //[Authorize]
    public class HomeController : System.Web.Mvc.Controller
    {
        public ActionResult Index()
        { 
        
            ViewBag.Title = "Home Page";
            return View();


        }

      
    }

    

    public class BasicRequest
    {
        public string test { get; set; }
    }

    public class BasicResponse
    {
        public string test { get; set; }
    }
}
