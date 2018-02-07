using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WIM.Core.Common.Utility.Helpers
{
    public class DownloadHelper
    {
        public static bool DownloadFile(string path, string fileName)
        {
            if (!File.Exists(path))
                return false;

            System.Web.HttpResponse respone = System.Web.HttpContext.Current.Response;
            respone.ClearContent();
            respone.Clear();
            respone.AppendHeader("content-disposition", "attachment; filename=" + fileName);
            //respone.ContentType = "text/plain";
            respone.ContentType = "Application/msword";
            respone.WriteFile(path);
            respone.Flush();
            respone.Close();

            return true;
        }
    }
}
