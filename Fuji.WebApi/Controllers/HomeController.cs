using Fuji.Service.Impl.ItemImport;
using Fuji.Service.Impl.PrintLabel;
using Fuji.Service.ItemImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fuji.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IItemImportService ser = new ItemImportService();
            //ser.Test();

            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
