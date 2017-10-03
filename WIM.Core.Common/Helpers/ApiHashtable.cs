using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WIM.Core.Security.Context;

namespace WIM.Core.Common.Helpers
{
    public static class ApiHashtable
    {
        private static SecurityDbContext db;
        public static Hashtable apiTable;

        public static void Initialize()
        {
            db = new SecurityDbContext();
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