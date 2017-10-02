using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Master;

namespace WMS.WebApi
{
    public static class ApiHashtable
    {
        private static MasterContext db = MasterContext.Create();
        public static Hashtable apiTable;

        public static void Initialize()
        {
            apiTable = new Hashtable();

            var api = from row in db.Api_MT
                      select row;

            foreach(var a in api)
            {
                apiTable.Add(a.ApiIDSys, a.Api);
            }
        }
    }
}