using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WIM.Core.Security.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Context;

namespace WIM.Core.Common.Helpers
{
    public class ApiHashTableHelper
    {
        private static CoreDbContext db;
        public static Hashtable apiTable;

        public static void Initialize()
        {
            db = new CoreDbContext();
            apiTable = new Hashtable();

            // #JobComment
            var api = from row in db.Api_MT
                      select row;

            foreach(var a in api)
            {
                apiTable.Add(a.ApiIDSys, a.Api);
            }
        }
    }
}